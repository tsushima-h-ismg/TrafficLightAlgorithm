using System;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_SetSec : Form
    {
        /// <summary>
        /// 進行可能秒数の最大値
        /// </summary>
        private const int AvaiSecMax   = 20;

        /// <summary>
        /// 進行可能秒数の最小値
        /// </summary>
        private const int AvaiSecMin   = 5;

        /// <summary>
        /// 矢印信号機の点灯秒数の最大値
        /// </summary>
        private const int ArrowSecMax  = 5;

        /// <summary>
        /// 矢印信号機の点灯秒数の最小値
        /// </summary>
        private const int ArrowSecMin  = 1;

        /// <summary>
        /// 進行可能秒数
        /// </summary>
        public int AvaiSec;

        /// <summary>
        /// 矢印信号機の点灯秒数
        /// </summary>
        public int ArrowSec;

        /// <summary>
        /// 方角を表す文字列
        /// </summary>
        public string DirectionName;

        /// <summary>
        /// 設定値の変更が有効の場合はtrue、それ以外の場合はfalse
        /// </summary>
        public bool IsEnable;

        /// <summary>
        /// 矢印信号機を持つ場合はtrue、それ以外の場合はfalse
        /// </summary>
        public bool IsArrow;

        public F_SetSec()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        private void F_SetSec_Load(object sender, EventArgs e)
        {
            try
            {
                bool arrowEnable = IsEnable;          // 矢印信号機点灯秒数を入力するテキストボックスのEnabledプロパティ値
                if (IsEnable) arrowEnable = IsArrow;  // IsEnableがtrueの場合に矢印信号機が存在するか判定する

                lbl_SoftTitle.Text   = DirectionName + "信号機の設定値";  // フォームタイトルを取得

                txt_AvaiSec.Text     = AvaiSec.ToString();   // 進行可能秒数を取得
                txt_AvaiSec.Enabled  = IsEnable;             // 進行可能秒数入力欄テキストボックスのEnabledプロパティ値設定

                txt_ArrowSec.Text    = ArrowSec.ToString();  // 矢印信号機点灯秒数を取得
                txt_ArrowSec.Enabled = arrowEnable;          // 矢印信号機点灯秒数テキストボックスのEnabledプロパティ値設定
            }
            catch (Exception ex)
            {
                string exStr = "設定値入力フォームのロードでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 確定ボタンクリック時イベント
        /// </summary>
        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                string errMsg = "";  // エラーメッセージが入る

                // テキストボックスに入力した値のチェック
                if (!CheckSecText(txt_AvaiSec,  out int avaiVal))  errMsg += $"「進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
                if (!CheckSecText(txt_ArrowSec, out int arrowVal)) errMsg += $"「矢印信号機の点灯時間」には{ArrowSecMin}から{ArrowSecMax}の整数を入力してください。\n";

                if (errMsg == "")
                {
                    AvaiSec  = avaiVal;   // テキストボックスに入力した進行可能秒数を取得する
                    ArrowSec = arrowVal;  // テキストボックスに入力した矢印信号機の点灯秒数を取得する
                    Close();
                }
                else
                {
                    MessageBox.Show(errMsg, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);  // エラーメッセージ表示
                }
            }
            catch (Exception ex)
            {
                string exStr = "「確定」ボタンクリックでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// テキストボックスのTextプロパティ値のチェックを行う
        /// </summary>
        /// <param name="txtBox">    チェック対象のテキストボックス              </param>
        /// <param name="resultVal"> txtBoxのTextプロパティ値をint型に変換した値 </param>
        /// <returns> txtBoxのTextプロパティ値をint型に変換した値が最大値と最小値の範囲を満たす場合はtrue、それ以外の場合はfalse </returns>
        private bool CheckSecText(TextBox txtBox, out int resultVal)
        {
            try
            {
                resultVal = 0;

                int maxVal = AvaiSecMax;  // 進行可能秒数の最大値を取得
                int minVal = AvaiSecMin;  // 進行可能秒数の最小値を取得

                // チェック対象テキストボックスが矢印信号機点灯秒数の入力欄の場合
                if (txtBox == txt_ArrowSec)
                {
                    maxVal = ArrowSecMax;  // 矢印信号機点灯秒数の最大値を取得
                    minVal = ArrowSecMin;  // 矢印信号機点灯秒数の最小値を取得
                }

                if (!double.TryParse(txtBox.Text, out double dbevalue)) return false;  // Textプロパティ値がdouble型に変換できない場合は終了する
                if (!int.TryParse(dbevalue.ToString(), out resultVal))  return false;  // double型から変換した文字列がint型に変換できない場合は終了する
                if (resultVal < minVal || resultVal > maxVal)           return false;  // int型に変換した値がminValより小さい、もしくはmaxValより大きい場合は終了する
                return true;
            }
            catch
            {
                resultVal = 0;
                return false;
            }
        }
    }
}