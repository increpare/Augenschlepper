var fs = require('fs'),
PNG = require('pngjs').PNG;

var data = fs.readFileSync('font.png');
var png = PNG.sync.read(data);

var buchstaben = `
1234567890
abcdefghijklmnopqrstuvwxyz
äöü[],.!;
`.trim();

var font={};

var cw=8;
var ch=8;


	function rotUZS(n){
		switch(n){
			case 0://up
				return 3;
			case 1://down
				return 2;
			case 2://left
				return 0;
			case 3://right
				return 1;
		}
	}

	function rotGUZS(n){
		switch(n){
			case 0://up
				return 2;
			case 1://down
				return 3;
			case 2://left
				return 1;
			case 3://right
				return 0;
		}
	}

	function nebenKoord(x,y,d){//right von der punkt
		switch(d){
			case 0://u
				return [x,y-1];
			break;
			case 1://d
				return [x-1,y];
			break;
			case 2://l
				return [x-1,y-1];
			break;
			case 3://r
				return [x,y];
			break;
		}
	}

	function dirToDelta(d){
		switch(d){
			case 0://u
				return [0,-1];
			break;
			case 1://d
				return [0,1];
			break;
			case 2://l
				return [-1,0];
			break;
			case 3://r
				return [1,0];
			break;
		}
	}

	function getPunkt(x,y,raster){
		if (x<0||y<0||x>=raster[0].length||y>=raster.length){
			return 0;
		}
		return raster[y][x];
	}


	function removeKante(kante,kanten){
		for (var i=0;i<kanten.length;i++){
			var k = kanten[i];
			if (k[0]===kante[0]&&k[1]===kante[1]&&k[2]===kante[2]){
				kanten.splice(i,1);
				return;
			}
		}
		console.log("error kante not found")
	}
	function Konturausrechnung(ebene,sx,sy,sdir,kanten){
		var x = sx;
		var y = sy;

		var dir = sdir;

		var coords=[[x,y,dir]];
		//im Uhrzeigersinn
		do {

			let [dx,dy]=dirToDelta(dir);
			
			var [nx,ny] = nebenKoord(x,y,dir);
			removeKante([nx,ny,rotGUZS(dir)],kanten);

			x+=dx;
			y+=dy;
			var ldir = rotGUZS(dir)
			
			var [nk_l_x,nk_l_y]=nebenKoord(x,y,ldir);
			
			var [nk_x,nk_y]=nebenKoord(x,y,dir);

			if (getPunkt(nk_l_x,nk_l_y,ebene)===1){
				dir=ldir;				
			} else if (getPunkt(nk_x,nk_y,ebene)===1){
				//dir=gleich
			} else {
				dir = rotUZS(dir);
			}

			coords.push([x,y,dir]);
		} while (x!=sx || y!=sy || dir!=sdir)

		return coords;
	}


function Kanten(mauermaske){
	var result=[];
	for (var j=0;j<mauermaske.length;j++){
		var zeile=mauermaske[j];
		for (var i=0;i<zeile.length;i++){
			var c = zeile[i];
			if (c===0){
				continue;
			}
			var c_oben = getPunkt(i,j-1,mauermaske);
			var c_unten = getPunkt(i,j+1,mauermaske);
			var c_links = getPunkt(i-1,j,mauermaske);
			var c_rechts = getPunkt(i+1,j,mauermaske);
			if (c_oben===0){
				result.push([i,j,0]);
			}
			if (c_unten===0){
				result.push([i,j,1]);
			}
			if (c_links===0){
				result.push([i,j,2]);
			}
			if (c_rechts===0){
				result.push([i,j,3]);
			}
		}
	}
	return result;
}

function simplify(linie){
	for (var i=0;i<linie.length-2;i++){
		var p1 = linie[i];
		var p2 = linie[i+1];
		var p3 = linie[i+2];
		if ( (p1[0]===p2[0] && p1[0]===p3[0]) || (p1[1]===p2[1] && p1[1]===p3[1]) ){
			linie.splice(i+1,1);
			i--;
		}
	}
	linie.pop();
}

var lines = buchstaben.split("\n");
for (j=0;j<lines.length;j++){
	var row=lines[j];
	for (var i=0;i<row.length;i++){
		var c = row.charAt(i);

		var raster=[];
		for (var gy=0;gy<ch;gy++){
			zeile=[];
			for (var gx=0;gx<cw;gx++){
				var pixel_x = i*cw+gx;
				var pixel_y = j*ch+gy;
				var pixel_idx = (png.width * pixel_y + pixel_x) << 2
				var pixel = png.data[pixel_idx];
				if (pixel>=128){
					zeile.push(1);
				} else {
					zeile.push(0);
				}
			}
			raster.push(zeile);
		}

		var kanten = Kanten(raster);
		var konturen=[];
		while(kanten.length>0){
				let [x,y,dir]=kanten[0];
				dir = rotUZS(dir);
				switch(dir){
					case 0:
					y++;
					break;
					case 1:
					x++;
					break;
					case 2:
					x++;
					y++;
					break;
					case 3:
					break;
				}
				var ol = kanten.length;
				var linie = Konturausrechnung(raster,x,y,dir,kanten);
				simplify(linie);
				konturen.push(linie);
				if (kanten.length===ol){
					break;
				}
		}

		font[c]=konturen;
	}
}

console.log(JSON.stringify(font));