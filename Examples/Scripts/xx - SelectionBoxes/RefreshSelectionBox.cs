using UnityEngine;
using System.Collections;

namespace MaterialUI
{
	public class RefreshSelectionBox : MonoBehaviour
	{
		[SerializeField] private SelectionBoxConfig box;

		public void Refresh ()
		{
			box.listItems = new string[6];

			box.listItems[0] = "D";
			box.listItems[1] = "E";
			box.listItems[2] = "F";
			box.listItems[3] = "G";
			box.listItems[4] = "H";
			box.listItems[5] = "I";

			box.RefreshList();
		}
	}
}