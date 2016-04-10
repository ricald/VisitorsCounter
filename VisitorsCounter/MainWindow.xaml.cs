//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="CompanyName">
//     Copyright © 2016 Ricald All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace VisitorsCounter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>カウンタの最小値</summary>
        private const int CountMin = 0;

        /// <summary>カウンタの最大値</summary>
        private const int CountMax = 999;

        /// <summary>コントロールのリスト</summary>
        private List<ControlItems> controlList;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.controlList = new List<ControlItems>()
            {
                { new ControlItems(this.buttonUp10, this.buttonDown10, this.textTitle10, this.textCount10, 0) },
                { new ControlItems(this.buttonUp20, this.buttonDown20, this.textTitle20, this.textCount20, 0) },
                { new ControlItems(this.buttonUp30, this.buttonDown30, this.textTitle30, this.textCount30, 0) },
                { new ControlItems(this.buttonUp40, this.buttonDown40, this.textTitle40, this.textCount40, 0) },
                { new ControlItems(this.buttonUp50, this.buttonDown50, this.textTitle50, this.textCount50, 0) },
                { new ControlItems(this.buttonUp60, this.buttonDown60, this.textTitle60, this.textCount60, 0) },
                { new ControlItems(this.buttonUp70, this.buttonDown70, this.textTitle70, this.textCount70, 0) },
                { new ControlItems(this.buttonUp80, this.buttonDown80, this.textTitle80, this.textCount80, 0) },
                { new ControlItems(this.buttonUp90, this.buttonDown90, this.textTitle90, this.textCount90, 0) },
            };

            this.Load();
        }

        /// <summary>
        /// カウントアップボタンクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">通知元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ButtonCountUp_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null)
            {
                return;
            }

            ControlItems items = this.controlList.Find((elem) => elem.ButtonUp == clickedButton);
            if (items == null)
            {
                return;
            }

            if (items.Count >= CountMax)
            {
                return;
            }

            items.Count++;
            items.TextCount.Text = items.Count.ToString(System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// カウントダウンボタンクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">通知元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ButtonCountDown_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null)
            {
                return;
            }

            ControlItems items = this.controlList.Find((elem) => elem.ButtonDown == clickedButton);
            if (items == null)
            {
                return;
            }

            if (items.Count <= CountMin)
            {
                return;
            }

            items.Count--;
            items.TextCount.Text = items.Count.ToString(System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// リセットボタンクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">通知元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show(Properties.Resources.MessageConfirmReset, Properties.Resources.CaptionConfirm, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            foreach (var items in this.controlList)
            {
                items.Count = 0;
                items.TextCount.Text = items.Count.ToString(System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// 閉じるボタンクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">通知元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ウィンドウClosedイベントハンドラ
        /// </summary>
        /// <param name="sender">通知元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// カウンタの読み込み
        /// </summary>
        private void Load()
        {
            if (System.IO.File.Exists("Count.csv") == false)
            {
                return;
            }

            using (System.IO.StreamReader sr = new System.IO.StreamReader("Count.csv"))
            {
                sr.ReadLine();
                string dataLine = sr.ReadLine();
                string[] dataArray = dataLine.Split(',');
                for (int index = 0; index < this.controlList.Count; index++)
                {
                    if (index < dataArray.Length)
                    {
                        int count;
                        if (int.TryParse(dataArray[index], out count) == false)
                        {
                            count = 0;
                        }

                        this.controlList[index].Count = count;
                        this.controlList[index].TextCount.Text = count.ToString(System.Globalization.CultureInfo.CurrentCulture);
                    }
                }
            }
        }

        /// <summary>
        /// カウンタの保存
        /// </summary>
        private void Save()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("Count.csv", false))
            {
                StringBuilder sb = new StringBuilder();

                sb.Clear();
                foreach (var items in this.controlList)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(items.TextTitle.Text);
                }

                sw.WriteLine(sb.ToString());

                sb.Clear();
                foreach (var items in this.controlList)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(items.Count.ToString(System.Globalization.CultureInfo.CurrentCulture));
                }

                sw.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// コントロールアイテムクラス
        /// </summary>
        private class ControlItems
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="buttonUp">カウントアップボタン</param>
            /// <param name="buttonDown">カウントダウンボタン</param>
            /// <param name="textTitle">タイトルテキスト</param>
            /// <param name="textCount">カウント数テキスト</param>
            /// <param name="count">カウント数</param>
            public ControlItems(Button buttonUp, Button buttonDown, TextBlock textTitle, TextBlock textCount, int count)
            {
                this.ButtonUp = buttonUp;
                this.ButtonDown = buttonDown;
                this.TextTitle = textTitle;
                this.TextCount = textCount;
                this.Count = count;
            }

            /// <summary>カウントアップボタン</summary>
            public Button ButtonUp { get; set; }

            /// <summary>カウントダウンボタン</summary>
            public Button ButtonDown { get; set; }

            /// <summary>タイトルテキスト</summary>
            public TextBlock TextTitle { get; set; }

            /// <summary>カウント数テキスト</summary>
            public TextBlock TextCount { get; set; }

            /// <summary>カウント数</summary>
            public int Count { get; set; }
        }
    }
}
