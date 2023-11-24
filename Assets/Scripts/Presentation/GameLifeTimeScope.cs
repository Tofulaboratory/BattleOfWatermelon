using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{

    [SerializeField] private TitleView _titleView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<FruitFactory>(Lifetime.Scoped);    
        builder.RegisterEntryPoint<GameInitializer>();
        builder.RegisterInstance(_titleView).As<ITitleView>();
    }
}