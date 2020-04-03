using Prism.Navigation;
using System;
using MvvmSample3.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace MvvmSample3.ViewModels
{
    /// <summary>
    /// MainPageのViewModel
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        // ReactivePropetyオブジェクト、ReactiveCommandオブジェクトをまとめてDisposeするためのもの
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        // プロパティ
        public ReactiveProperty<int> Number { get; }

        // デクリメントコマンド
        //public ReactiveCommand DecrementCommand { get; } = new ReactiveCommand();
        public AsyncReactiveCommand DecrementCommand { get; } = new AsyncReactiveCommand();

        // インクリメントコマンド
        //public ReactiveCommand IncrementCommand { get; } = new ReactiveCommand();
        public AsyncReactiveCommand IncrementCommand { get; } = new AsyncReactiveCommand();

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
            DecrementCommand.Subscribe(() => Task.Run(model.Decrement));
            // インクリメントコマンド実行時の処理
            IncrementCommand.Subscribe(() => Task.Run(model.Increment));
        }

        /// <summary>
        /// CompositeDisposableに追加したオブジェクトをまとめてDisposeする
        /// </summary>
        public override void Destroy()
        {
            Disposable.Dispose();
        }
    }
}
