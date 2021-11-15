namespace BTEditor
{
    [System.Serializable]
    public class BehaviorData
    {

        public int id;
        public int[] dependencies;
        public string name;
        public BehaviorType type;
    }

    public enum BehaviorType
    {
        Selector,
        Sequence,
        Node
    }
}