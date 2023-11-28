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
        builder.Register<GameFactory>(Lifetime.Scoped);
        builder.Register<GameBoardFactory>(Lifetime.Scoped);
        builder.Register<PlayerFactory>(Lifetime.Scoped);
        builder.Register<FruitFactory>(Lifetime.Scoped);

        builder.Register<GameRegistry>(Lifetime.Scoped);

        builder.RegisterEntryPoint<GameInitializer>();
        builder.Register<GameUsecase>(Lifetime.Scoped);
    
        builder.RegisterInstance(_titleView).As<ITitleView>();
    }
}