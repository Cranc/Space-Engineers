///<summary> Managed Refinery Extension script moves refinery output to given containers
///<author> Cranc - Debauchery Engineer Tea Party

//default container name
string container_name = "Container Ingots";

///<param name="name"> name of the container
///<param name="exact"> true when parameter name needs to match exact the name of the container
void Main(String argument)
{
    // init stuff + default container
    bool exact = false;

    if(argument != null){
        var pos = argument.IndexOf("name:");
        if(pos != -1){
            var npos = argument.IndexOf("exact:");
            if(npos != -1){
                var temp = argument.Substring(pos + 6);
                if(temp.Contains("true") || temp.Contains("1") || temp.Contains("True"))
                    exact = true;

                container_name = argument.Substring(pos + 5, npos - 6);
            }else{
                container_name = argument.Substring(pos + 5);
            }
        }
    }

    /// get refinerys
    var refinery_list = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyRefinery>(refinery_list);

    if(refinery_list.Count == 0){
        throw new Exception("no refinery found ... Someone fucked uuuup");
    }

    var container_list = new List<IMyTerminalBlock>();

    if(!exact){
        // get container contain at least the string
        GridTerminalSystem.SearchBlocksOfName(container_name, container_list, FilterMethod);
    }else{
        // get container contain exact string
        GridTerminalSystem.SearchBlocksOfName(container_name, container_list, ExactFilterMethod);
    }

    if(container_list.Count == 0){
        throw new Exception("no container found ... Someone fucked uuuup");
    }

    /// get inventorys

    var refinery_inventory_list = new List<IMyInventory>();
    var container_inventory_list = new List<IMyInventory>();

    //// get all refinery output inventorys
    for(int i = 0; i < refinery_list.Count; i++){
        refinery_inventory_list.Add(refinery_list[i].GetInventory(1));
    }
    if(refinery_inventory_list.Count == 0){
        throw new Exception("refinery inventory list empty ... huh???");
    }

    //// get all container inventorys
    for(int i = 0; i < container_list.Count; i++){
        container_inventory_list.Add(container_list[i].GetInventory(0));
    }
    if(container_inventory_list.Count == 0){
        throw new Exception("container inventory list empty ... huh???");
    }

    // init end

    // main
    for(int i = 0; i < refinery_inventory_list.Count; i++){
        int container_Id = 0;
        var temp_item_list = new List<IMyInventoryItem>();
        temp_item_list = refinery_inventory_list[i].GetItems();
        for(int j = 0; j < temp_item_list.Count; j++){
            if(!refinery_inventory_list[i].IsConnectedTo(container_inventory_list[container_Id]))
                    throw new Exception("inventorys are not connected ... you should check conveyer system .... god -.-");

            var result = refinery_inventory_list[i].TransferItemTo(container_inventory_list[container_Id], 0, stackIfPossible: true);
            if (!result){
                    container_Id++;
                if (container_Id >= container_inventory_list.Count){
                    break;
                }
            }
        }
    }
}

///<param name="block"> block to be checked
bool FilterMethod(IMyTerminalBlock block)
{
    IMyCargoContainer cargo = block as IMyCargoContainer;
    return cargo != null;
}

///<param name="block"> block to be checked
bool ExactFilterMethod(IMyTerminalBlock block)
{
    IMyCargoContainer cargo = block as IMyCargoContainer;
    if(cargo != null){
        if(cargo.CustomName.Equals(container_name))
            return true;
    }
    return false;
}