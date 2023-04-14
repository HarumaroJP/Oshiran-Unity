using System.Collections;
using System.Collections.Generic;
using Oshiran.Stage;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TitleLifeTimeScope : LifetimeScope
{
    [SerializeField] DataReset dataReset;
    [SerializeField] ProgressHistoryView progressHistoryView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<ProgressHistoryPresenter>();

        builder.RegisterComponent(progressHistoryView);
        builder.RegisterComponent(dataReset);
    }
}