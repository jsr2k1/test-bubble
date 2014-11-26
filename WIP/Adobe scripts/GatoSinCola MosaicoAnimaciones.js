// Enable double clicking from the Macintosh Finder or the Windows Explorer
#target photoshop

// in case we double clicked the file
app.bringToFront();

//var logStream = File("/c/MosaicoAnimaciones.log");
//logStream.open('w');

// debug level: 0-2 (0:disable, 1:break on error, 2:break at beginning)
// $.level = 0;
// debugger; // launch debugger on next line

// suppress all dialogs
app.displayDialogs = DialogModes.NO;

var strtRulerUnits = app.preferences.rulerUnits;
var strtTypeUnits = app.preferences.typeUnits;
app.preferences.rulerUnits = Units.PIXELS;
app.preferences.typeUnits = TypeUnits.POINTS;

var iSourceSize = 256;
var iRows = 9;
var iCols = 9;

//Create a new document
var targetDoc = app.documents.add(iSourceSize*iCols, iSourceSize*iRows, 72, "myDocument", NewDocumentMode.RGB);
var sourceDoc = app.documents[0];

//Loop through layers
for (var i=0; i<sourceDoc.artLayers.length; i++)
{
	var theLayer = sourceDoc.artLayers[i];
	app.activeDocument = sourceDoc;
	var layerRef = theLayer.duplicate(targetDoc, ElementPlacement.PLACEATBEGINNING);
	app.activeDocument = targetDoc;
	var x = iSourceSize*(i%iCols);
	var y = (iRows-Math.floor(i/iCols)-1);
	y=y*iSourceSize;
	layerRef.translate(x,y);
}

//logStream.write(sourceDoc);
//logStream.close();


