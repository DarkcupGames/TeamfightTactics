using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MinionType : ScriptableObjectInstaller<MinionType>
{
    public string displayName = "name";
    public Sprite icon;

    public override void InstallBindings()
    {
        Container.BindInstance(this);
    }

}
