using System.Collections;
using System.Collections.Generic;
using Oshiran.Stage;
using Oshiran.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TitleLifeTimeScope : LifetimeScope
{
    [SerializeField] DataReset dataReset;
    [SerializeField] ProgressHistoryView progressHistoryView;
    [SerializeField] ButtonOpening buttonOpening;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<ProgressHistoryPresenter>();
        builder.RegisterEntryPoint<ButtonOpeningPresenter>();

        builder.RegisterComponent(progressHistoryView);
        builder.RegisterComponent(dataReset);
        builder.RegisterComponent(buttonOpening);
    }
}