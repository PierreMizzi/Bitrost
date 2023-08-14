
namespace PierreMizzi.Useful
{

	/*
	
		Every solution tried

		# Try N°4
		- hidden class on beginning
			- ID sets base scale and opacity
			- hidden class
			- toggle fade-in or fade-out classes 
		-> Total failure FFS

		# Try N°5
		- specific class for each animations
			- .[objectname]-fade-in & .[objectname]-fade-in
			- another class .animation
			- For exemple, .[objectname]-fade-in.animate

	
	*/
	public class UIToolkitUtils
	{
		// Try n°5
		public static string hidden = "hidden";

		public static string fadeIn = "fade-in";
		public static string fadeOut = "fade-out";

		public static string animate = "animate";
		public static string reset = "reset";
	}
}