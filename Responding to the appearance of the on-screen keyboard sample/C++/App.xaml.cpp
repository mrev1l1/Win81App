﻿// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

//
// App.xaml.cpp
// Implementation of the App.xaml class.
//

#include "pch.h"
#include "App.xaml.h"
#include "MainPage.xaml.h"

using namespace Windows::UI::Xaml;
using namespace Windows::Foundation;
using namespace Windows::ApplicationModel;
using namespace Windows::ApplicationModel::Activation;
using namespace CppSamplesUtils;
using namespace KeyboardEventsSampleCPP;
using namespace Windows::UI::Xaml::Interop;

App::App()
{
	InitializeComponent();
	this->Suspending += ref new SuspendingEventHandler(this, &KeyboardEventsSampleCPP::App::OnSuspending);
}

void App::OnSuspending(Platform::Object^ sender, Windows::ApplicationModel::SuspendingEventArgs^ pArgs)
{
    auto deferral = pArgs->SuspendingOperation->GetDeferral();
    SuspensionManager::StartSaveTask().then([deferral] () {
        deferral->Complete();
    });
}

void App::OnLaunched(Windows::ApplicationModel::Activation::LaunchActivatedEventArgs^ pArgs)
{
    if (pArgs->PreviousExecutionState == ApplicationExecutionState::Terminated)
    {
        SuspensionManager::StartRestoreTask().then([this] () {
            auto rootFrame = ref new Frame();
            TypeName pageType = { "KeyboardEventsSampleCPP.MainPage", TypeKind::Custom };
            rootFrame->Navigate(pageType);
            Window::Current->Content = rootFrame;
            Window::Current->Activate();
        });
    }
    else
    {
        auto rootFrame = ref new Frame();
        TypeName pageType = { "KeyboardEventsSampleCPP.MainPage", TypeKind::Custom };
        rootFrame->Navigate(pageType);
        Window::Current->Content = rootFrame;
        Window::Current->Activate();
    }
}
