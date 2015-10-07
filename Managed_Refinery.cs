
///<summary> Managed Refinery script makes sure that every refinery always has the same amount of ressources in production
///<author> Cranc - Debauchery Engineer Tea Party
void Main()
{
    var refinery_list = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyRefinery>(refinery_list);

    List<KeyValuePair<IMyTerminalBlock,  double>> ref_list;

    ref_list = new List<KeyValuePair<IMyTerminalBlock,  double>>();
	for (int i = 0; i < refinery_list.Count; i++ ){
        ref_list.Add(new KeyValuePair<IMyTerminalBlock,  double>(refinery_list[i], GetMassOfInventory(refinery_list[i])));
	}

	List<KeyValuePair<IMyTerminalBlock,  double>> mySortList = ref_list;

    if( mySortList != null) {
        mySortList.Sort((x,y)=>y.Value.CompareTo(x.Value));
        int end = mySortList.Count - 1;

        for (int start = 0; start < end; start++){
            var dif = mySortList[start].Value - mySortList[end].Value;
            dif /= 2;
            var inv_one = mySortList[start].Key.GetInventory(0);
            var inv_two = mySortList[end].Key.GetInventory(0);

            if( inv_one.IsConnectedTo(inv_two)) {
                //debug_list[0].SetCustomName(debug_list[0].CustomName + "inventory is connected ...");
                var itemList = new List<IMyInventoryItem>();
                mySortList[start].Key.GetInventory(0).GetItems();

                var theresult = mySortList[start].Key.GetInventory(0).TransferItemTo(
                    mySortList[end].Key.GetInventory(0),
                    0,
                    stackIfPossible: true,
                    amount: (VRage.MyFixedPoint)dif
                );
            }else{
                //throw new Exception("inventorys not connected");
            }
            end--;
        }
    }else{
        throw new Exception("no refinery in list");
    }
}

///<param name="myref"> refinery block
double GetMassOfInventory(IMyTerminalBlock myref)
{
    var input_inv = myref.GetInventory(0);

    return (double)(input_inv.CurrentMass);
}


