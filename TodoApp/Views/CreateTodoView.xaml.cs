using System.Collections.ObjectModel;
using System.Reactive.Linq;
using TodoApp.Application.cs.Todos.DTOs;
using TodoApp.ViewModels;

namespace TodoApp.Views;

public partial class CreateTodoView : ContentPage
{
    private List<IDisposable> _disposables;
    public CreateTodoView()
	{
		InitializeComponent();
        _disposables = new List<IDisposable>();
	}

    protected override void OnAppearing()
    {
        SetObservables();
        base.OnAppearing();
    }

    private void SetObservables()
    {
        SetEntry_TaskNameObservables();
        SetEditor_DescriptionObservables();
        SetDatePicker_PrevisionDateObservables();
    }

    private void SetDatePicker_PrevisionDateObservables()
    {
        var datePickerDateChangeObs = Observable.FromEventPattern<DateChangedEventArgs>(
            handler => DatePicker_TaskDate.DateSelected += handler,
            handler => DatePicker_TaskDate.DateSelected -= handler)
            .Select(e => e.EventArgs.NewDate);

        var sub = datePickerDateChangeObs
                                .Do(state =>
                                {
                                    DatePicker_TaskDate
                                    .Dispatcher
                                    .Dispatch(() =>
                                    {
                                        PrevisionDate_Label_Validation.IsVisible = (state.Date < DateTime.Now.Date);
                                    });
                                }).Subscribe();

        _disposables.Add(sub);
    }

    private void SetEditor_DescriptionObservables()
    {
        var editorTxtChangedObservable = Observable.FromEventPattern<TextChangedEventArgs>(
            handler => Editor_TaskDescription.TextChanged += handler,
            handler => Editor_TaskDescription.TextChanged -= handler)
            .Select(e => e.EventArgs.NewTextValue);

        var editorFocusChangedObservable = Observable.FromEventPattern<FocusEventArgs>(
            handler => Editor_TaskDescription.Focused += handler,
            handler => Editor_TaskDescription.Focused -= handler)
            .Select(e => e.EventArgs.IsFocused);

        var sub = editorTxtChangedObservable
                            .CombineLatest(editorFocusChangedObservable, (txt, isFocused) => new { txt, isFocused })
                            .Do(state =>
                            {
                                Editor_TaskDescription
                                .Dispatcher
                                .Dispatch(() =>
                                {
                                    Description_Label_Validation_Required.IsVisible = state.txt?.Length == 0;
                                    Description_Label_Validation.IsVisible = state.isFocused && (state.txt?.Length < 5);
                                });
                            })
                            .Subscribe();
        _disposables.Add(sub);
    }

    private void SetEntry_TaskNameObservables()
    {
        var entryTxtChangedObservable = Observable.FromEventPattern<TextChangedEventArgs>(
                handler => Entry_TaskName.TextChanged += handler,
                handler => Entry_TaskName.TextChanged -= handler)
            .Select(e => e.EventArgs.NewTextValue);
        var entryFocusChangedObservable = Observable.FromEventPattern<FocusEventArgs>(
                handler => Entry_TaskName.Focused += handler,
                handler => Entry_TaskName.Focused -= handler)
            .Select(e => e.EventArgs.IsFocused);

        var sub = entryTxtChangedObservable
                                .CombineLatest(entryFocusChangedObservable, (txt, isFocused) => new { txt, isFocused })
                                .Do(state =>
                                {
                                    Entry_TaskName
                                    .Dispatcher
                                    .Dispatch(() =>
                                    {
                                        Name_Label_Validation_Required.IsVisible = state.txt?.Length == 0;
                                        Name_Label_Validation.IsVisible = state.isFocused && (state.txt?.Length < 5);
                                    });
                                })
                                .Subscribe();
        _disposables.Add(sub);
    }

    protected override void OnDisappearing()
    {
        this._disposables.ForEach(disposable => disposable.Dispose());
        base.OnDisappearing();
    }
}