using Prism.Navigation;
using System;
using MvvmSample3.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;

namespace MvvmSample3.ViewModels
{
    /// <summary>
    /// MainPageのViewModel
    /// </summary>
    public class MainPageViewModel : ViewModelBase, IDisposable
    {
        // プロパティ
        public ReactiveProperty<int> Number { get; } = new ReactiveProperty<int>();

        // デクリメントコマンド
        public ReactiveCommand DecrementCommand { get; } = new ReactiveCommand();

        // インクリメントコマンド
        public ReactiveCommand IncrementCommand { get; } = new ReactiveCommand();

        // モデル
        private Counter model = new Counter();

        // オブジェクトをまとめてDisposeするためのモノ
        // ★ReactivePropertyとModelのプロパティを同期するときのおまじない
        private CompositeDisposable disposable = new CompositeDisposable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            // NumberはModel(Counter)のNumberプロパティと同期する
            // ★AddTo(disposable)はReactivePropertyとModelの同期をするときのおまじない
            Number = model.ObserveProperty(x => x.Number).ToReactiveProperty().AddTo(disposable);
            // デクリメントコマンド実行時の処理
            DecrementCommand.Subscribe(() => model.Decrement());
            // インクリメントコマンド実行時の処理
            IncrementCommand.Subscribe(() => model.Increment());
        }

        /// <summary>
        /// CompositeDisposableにためたオブジェクトをまとめてDisposeする
        /// ★ReactivePropertyとModelのプロパティを同期するときのおまじない
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}
