//List
var container_list;
var refinery_list;
var assembler_list;
var connector_list;

var container_name_list;

//weight for assembler input
const float Weight_Iron = 0.1f;
const float Weight_Gravel = 0.1f;
const float Weight_Gold = 0.1f;
const float Weight_Silver = 0.1f;
const float Weight_Cobalt = 0.1f;
const float Weight_Uranium = 0.1f;
const float Weight_Nickel = 0.1f;
const float Weight_Silicon = 0.1f;
const float Weight_Magnesium = 0.1f;
const float Weight_Platinum = 0.1f;
//name of container
//Ingots
const string Container_Iron = "";
const string Container_Gravel = "";
const string Container_Gold = "";
const string Container_Silver = "";
const string Container_Cobalt = "";
const string Container_Uranium = "";
const string Container_Nickel = "";
const string Container_Silicon = "";
const string Container_Magnesium = "";
const string Container_Platnium = "";
//Vanilla Components
const string Container_Bulletproof_Glass = "";
const string Container_Computer = "";
const string Container_Construction_Components = "";
const string Container_Detector_Components = "";
const string Container_Display = "";
const string Container_Explosives = "";
const string Container_Girder = "";
const string Container_Gravity_Generator_Components = "";
const string Container_Interior_Plate = "";
const string Container_Medical_Components = "";
const string Container_Metal_Grid = "";
const string Container_Motor = "";
const string Container_Power_Cell = "";
const string Container_Radio_Communication_Components = "";
const string Container_Reactor_Components = "";
const string Container_Steel_Plate = "";
const string Container_Steel_Tube_Large = "";
const string Container_Steel_Tube_Small = "";
const string Container_Solar_Cell = "";
const string Container_Thruster_Components = "";
//Mod Components
const string Container_Concrete_Slab = "";
const string Container_Arc_Reactor_Components = "";
const string Container_Arc_Fuel = "";
const string Container_APCR_Shell = "";
const string Container_HEKC_Shell = "";
const string Container_HE_Shell = "";

void Init(){
	container_list = new List<IMyTerminalBlock>(); 
    GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(container_list);
	
	refinery_list = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyRefinery>(refinery_list);
	
	connector_list = new List<IMyTerminalBlock>(); 
    GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(connector_list);
	
	assembler_list = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyAssembler>(assembler_list)
	
	container_name_list = new List<String>();
	for(int i = 0; i < container_list.Count; i++)
		container_name_list.Add(container_list[i].CustomName);
	
	
}
void Main(){

}
void Sort(){
	
}
void Sort_Container(){
	for(int i = 0; i < container_list.Count; i++){
		var inventory = new List<IMyInventoryItem>();
		inventory = (container_list[i].GetInventory(0)).GetItems();
		
		//var container_name = container_list[i].CustomName;
		
		for(int j = 0; j < inventory.Count; j++){
			var Item = GetItemName(inventory[j]);
			var pos = container_list[i].CustomName.IndexOf(Item);
			if(pos == -1){//itemname not in containername-> ITEM DOES NOT BELONG IN THAT CONTAINER
				//get list with all container that have substring of the item name
				var resultList = container_namer_list.FindAll(delegate(string s) { return s.Contains(Item); });
			}else if(pos == 0){//string empty for what reason what so ever O.o 
				
			}
		}
	}
}
//returns item name if Flag is set => 1 it returns full name of ore or ingot
///<param name="item"> Item to get name from
///<param name="flag"> Concat flag adds Ore/Ingot behind items (if not set returns only Ore/Ingot definition and not the SubtypeName)
String GetItemName(IMyInventoryItem item, bool flag){
	String temp;
	if(flag){
		temp = item.Content.SubtypeName;
		if(item is Sandbox.Common.ObjectBuilders.MyObjectBuilder_Ore)
			Concat(temp,"Ore");
		else if(item is Sandbox.Common.ObjectBuilders.MyObjectBuilder_Ingot)
			Concant(temp,"Ingot");
	}else{
		if(item is Sandbox.Common.ObjectBuilders.MyObjectBuilder_Ore)
			temp = "ore";
		else if(item is Sandbox.Common.ObjectBuilders.MyObjectBuilder_Ingot)
			temp = "Ingot";
		else
			temp = item.Content.SubtypeName;
	}
	return temp;
}
