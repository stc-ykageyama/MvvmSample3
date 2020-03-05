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
        // ReactivePropetyオブジェクト、ReactiveCommandオブジェクトをまとめてDisposeするためのもの
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        // プロパティ
        public ReactiveProperty<int> Number { get; } = new ReactiveProperty<int>();

        // デクリメントコマンド
        public ReactiveCommand DecrementCommand { get; } = new ReactiveCommand();

        // インクリメントコマンド
        public ReactiveCommand IncrementCommand { get; } = new ReactiveCommand();

        // モデル
        private Counter model = new Counter();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            // Numberプロパティの設定
            Number = model.ObserveProperty(x => x.Number)       // Model(Counter)のNameプロパティの変更を監視する
                .ToReactiveProperty()                           // 監視対象のプロパティが変更されたらPropertyChangedイベントを発行する(ReactiveProperty化)
                .AddTo(Disposable);                             // Dispose対象に追加する ※忘れないように!!
            // デクリメントコマンド実行時の処理
            DecrementCommand.Subscribe(() => model.Decrement()) // DecrementCommandの実行時処理を設定する
                .AddTo(Disposable);                             // Dispose対象に追加する ※忘れないように!!
            // インクリメントコマンド実行時の処理
            IncrementCommand.Subscribe(() => model.Increment()) // IncrementCommandの実行時処理を設定する
                .AddTo(Disposable);                             // Dispose対象に追加する ※忘れないように!!
        }

        /// <summary>
        /// CompositeDisposableに追加したオブジェクトをまとめてDisposeする
        /// </summary>
        public void Dispose()
        {
            Disposable.Dispose();
        }
    }
}
