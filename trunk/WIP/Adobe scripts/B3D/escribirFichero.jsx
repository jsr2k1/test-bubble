/////////////////////////////////////////////////////////////////////////////
#target photoshop

/////////////////////////////////////////////////////////////////////////////
function SnpCreateProgressBar() 
{
	this.windowRef = null;
}

/////////////////////////////////////////////////////////////////////////////
SnpCreateProgressBar.prototype.run = function()
{
	var retval = true;
	
	// Create a palette-type window (a modeless or floating dialog),
	var win = new Window("palette", "SnpCreateProgressBar", [150, 150, 600, 300]); 
	this.windowRef = win;
	// Add a panel to contain the components
	win.pnl = win.add("panel", [10, 10, 440, 100], "Click Start to move the Progress bar");

	// Add a progress bar with a label and initial value of 0, max value of 200.
	win.pnl.progBarLabel = win.pnl.add("statictext", [20, 20, 320, 35], "Progress");
	win.pnl.progBar = win.pnl.add("progressbar", [20, 35, 410, 60], 0, 200);

	// Add buttons
	win.goButton = win.add("button", [25, 110, 125, 140], "Start");
	win.resetButton = win.add("button", [150, 110, 250, 140], "Reset");
	win.doneButton = win.add("button", [310, 110, 410, 140], "Done");


	// Define behavior for the "Done" button
	win.doneButton.onClick = function ()
	{
		win.close();
	};
	
	// Define behavior for the "Start" button
	win.goButton.onClick = function ()
	{
		while(win.pnl.progBar.value < win.pnl.progBar.maxvalue)
		{
			// this is what causes the progress bar increase its progress
			win.pnl.progBar.value++; 
			$.sleep(10);
		}
	};

	// Define behavior for the "Reset" button
	win.resetButton.onClick = function()
	{ 
		// set the progress back to 0
		win.pnl.progBar.value = 0; 
	}

	// Display the window
	win.show();

	return retval;
}

///////////////////
//MAIN
if(typeof(SnpCreateProgressBar_unitTest) == "undefined")
{
    new SnpCreateProgressBar().run();
}
