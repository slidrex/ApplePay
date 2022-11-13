using System.Text.RegularExpressions;
using System.Linq;

namespace Pay.Debug
{
    public static class ConsoleCommandHandler
    {
        const string DefaultEntityPath = "Assets/Prefabs/Entities/";
        private static System.Collections.Generic.Dictionary<string, System.Action<ConsoleCommand>> Commands = new System.Collections.Generic.Dictionary<string, System.Action<ConsoleCommand>>()
        {
            ["tp"] = (ConsoleCommand command) =>
            {
                Entity[] entities = ParseSelector(command.DefaultExecuter, command.Arguments[0], out ParseStatus selectStatus);
                
                UnityEngine.Vector3 position = ParseVector(command.DefaultExecuter.transform.position, command.Arguments[1], command.Arguments[2], "0", out ParseStatus vectorStatus);
                
                foreach(Entity entity in entities)
                {
                    entity.transform.position = position;
                }
            },
            ["spawn"] = (ConsoleCommand command) =>
            {
                UnityEngine.GameObject gameObject = ParseAsset(DefaultEntityPath + command.Arguments[0] + ".prefab", out ParseStatus status);
                
                UnityEngine.Vector3 position = ParseVector(command.DefaultExecuter.transform.position, command.Arguments[1], command.Arguments[2], "0", out ParseStatus vectorStatus);

                UnityEngine.MonoBehaviour.Instantiate(gameObject, position, UnityEngine.Quaternion.identity);
            },
            ["effect"] = (ConsoleCommand command) =>
            {
                Entity[] entities = ParseSelector(command.DefaultExecuter, command.Arguments[1], out ParseStatus status);
                if(command.Arguments[0] == "get")
                {
                    foreach(Entity entity in entities)
                    {
                        PayWorld.EffectController.AddEffect(entity, command.Arguments[2], byte.Parse(command.Arguments[3]), float.Parse(command.Arguments[4]));
                    }
                }
                else if(command.Arguments[0] == "clear")
                {
                    foreach(Entity entity in entities)
                    {
                        byte[] IDs = entity.ActiveEffects.Keys.ToArray();
                        for(int i = 0; i < IDs.Length; i++)
                        {
                            PayWorld.EffectController.RemoveEffect(entity, ref IDs[i]);
                        }
                        
                    }
                }
            }
        };
        public static void ExecuteCommand(Entity defaultExecuter, string[] args, out string output)
        {
            string[] _args = new string[args.Length - 1];
            for(int i = 0; i < _args.Length; i++) _args[i] = args[i + 1];
            bool isFound = Commands.TryGetValue(args[0], out System.Action<ConsoleCommand> val);
            
            if(isFound == false) 
            {
                output = "Command \"" + "/" + args[0] + "\" does not exist (write \"/help\" to check available commands).";
                return;
            }
            output = "Command executed!";
            val.Invoke(new ConsoleCommand(defaultExecuter, _args));
        }
        public static string[] ParseCommand(string command)
        {
            Regex regex = new Regex("/|[^ ]+");
            
            MatchCollection collection = regex.Matches(command);
            string[] strs = new string[collection.Count];
            for(int i = 0; i < strs.Length; i++)
            {
                strs[i] = collection[i].Value;
            }
            return strs;
        }
        private static UnityEngine.GameObject ParseAsset(string path, out ParseStatus status)
        {
            //UnityEngine.GameObject gameObject = (UnityEngine.GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.GameObject));
            
            //status = gameObject != null ? ParseStatus.Success : ParseStatus.Fail;
            status = 0;
            return null;
            //return gameObject;
        }
        private static Entity[] ParseSelector(Entity executer, string selector, out ParseStatus status)
        {
            status = ParseStatus.Success;
            switch(selector)
            {
                case "@me":
                Entity[] entity = {executer };
                return entity;
                case "@all":
                    return UnityEngine.MonoBehaviour.FindObjectsOfType<Entity>();
                default:
                    status = status = ParseStatus.Fail;
                    break;
            }
            return null;
        }
        private static UnityEngine.Vector3 ParseVector(UnityEngine.Vector3 relativePosition, string x, string y, string z, out ParseStatus status)
        {
            float xCoordinate = 0, yCoordinate = 0, zCoordinate = 0;
            
            if(x[0] == '~') {xCoordinate = relativePosition.x; x = x.Substring(1); if(x == "") x = "0";}
            if(y[0] == '~') {yCoordinate = relativePosition.y; y = y.Substring(1); if(y == "") y = "0";}
            if(z[0] == '~') {yCoordinate = relativePosition.z; z = z.Substring(1); if(z == "") z = "0";}
            try
            {
                xCoordinate += float.Parse(x);
                yCoordinate += float.Parse(y);
                zCoordinate += float.Parse(z);
            }
            catch
            {
                status = ParseStatus.Fail;
            }
            status = ParseStatus.Success;
            return new UnityEngine.Vector3(xCoordinate, yCoordinate, zCoordinate);
        }
        private enum ParseStatus
        {
            Success,
            Fail
        }
    }
    
    public class ConsoleCommand
    {
        public Entity DefaultExecuter;
        public string[] Arguments;
        public ConsoleCommand(Entity defaultExecuter, string[] arguments)
        {
            DefaultExecuter = defaultExecuter;
            Arguments = arguments;
        }
    }
}