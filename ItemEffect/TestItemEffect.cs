using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemEffect : ItemEffect
{
    public override void ApplyEffect(Character _character)
    {
        
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {

    }
    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();
        return _string;
    }

    public override void UndoEffect(Character _character)
    {
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }
}
