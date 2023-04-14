using System.Collections;
using System.Collections.Generic;
using Oshiran.Core;
using Oshiran.Module;
using Oshiran.Stage;
using Oshiran.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    [SerializeField] PlayerController controller;
    [SerializeField] AudioClipPlayer audioClipPlayer;
    [SerializeField] GameEventRelay gameEventRelay;
    [SerializeField] TutorialSequencer tutorialSequencer;
    [SerializeField] ProgressViewHandler progressViewHandler;
    [SerializeField] OnaraMetorView onaraMetorView;
    [SerializeField] SettingView settingView;
    [SerializeField] StageMover stageMover;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<PlayerSetup>();
        builder.RegisterEntryPoint<TutorialPresenter>();
        builder.RegisterEntryPoint<AudioPresenter>();
        builder.RegisterEntryPoint<ProgressSequencer>();

        builder.RegisterComponent(controller);
        builder.RegisterComponent(audioClipPlayer);
        builder.RegisterComponent(gameEventRelay);
        builder.RegisterComponent(tutorialSequencer);
        builder.RegisterComponent(onaraMetorView);
        builder.RegisterComponent(settingView);
        builder.RegisterComponent(progressViewHandler);
        builder.RegisterComponent(stageMover);
    }
}