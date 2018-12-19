/*


        <circle id="tw_1_00" class="tbs-8" cx="3.25" cy="40.25" r="1"/>

        <circle id="tw_2_00" class="tbs-8" cx="3.25"  cy="22.25" r="1"/>

        <circle id="tw_2_00" class="tbs-8" cx="3.25"  cy="4.25" r="1"/>
*/

var xcoords = [3.25,82.25]
var ycoords = [40.25,22.25,4.25]
var diff = 4;

var result="";
for (var i=0;i<xcoords.length;i++){
	for (var j=0;j<ycoords.length;j++){
		for (var x=0;x<3;x++){
			for (var y=0;y<3;y++){
				var cx = xcoords[i]+x*diff;
				var cy = ycoords[j]+y*diff;
				result+=`<circle id="tw_${j*xcoords.length+i}_${x}${y}" class="tbs-8" cx="${cx}" cy="${cy}" r="1"/>\n`;
			}
		}
	}
}

console.log(result);