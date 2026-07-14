using System;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_SetAllValue : Form
    {
        /// <summary>
        /// 進行可能秒数の最大値
        /// </summary>
        private const int AvaiSecMax  = 20;

        /// <summary>
        /// 進行可能秒数の最小値
        /// </summary>
        private const int AvaiSecMin  = 1;

        /// <summary>
        /// 矢印信号機の点灯秒数の最大値
        /// </summary>
        private const int ArrowSecMax = 5;

        /// <summary>
        /// 矢印信号機の点灯秒数の最小値
        /// </summary>
        private const int ArrowSecMin = 1;

        /// <summary>
        /// ミリ秒設定値
        /// </summary>
        public TrafficMSecValues SetMSecValues;

        public F_SetAllValue()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        private void F_SetAllValue_Load(object sender, EventArgs e)
        {
            try
            {
                txt_AvaiCN.Text     = (SetMSecValues.CarNMSec  / 1000).ToString();
                txt_AvaiCS.Text     = (SetMSecValues.CarSMSec  / 1000).ToString();
                txt_AvaiCE.Text     = (SetMSecValues.CarEMSec  / 1000).ToString();
                txt_AvaiCW.Text     = (SetMSecValues.CarWMSec  / 1000).ToString();
                txt_AvaiPNS.Text    = (SetMSecValues.PedNSMSec / 1000).ToString();
                txt_AvaiPEW.Text    = (SetMSecValues.PedEWMSec / 1000).ToString();
                txt_LightOnArw.Text = (SetMSecValues.ArwMSec   / 1000).ToString();
            }
            catch (Exception ex)
            {
                string exStr = "設定値入力画面の読み込みでエラーが発生しました。\n" + ex.Message;
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

                // 進行可能時間の入力チェック
                if (!CheckSecText(txt_AvaiCN, out int carNMSec)) errMsg += $"「北車用信号機の進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を\n入力してください。\n";
                if (!CheckSecText(txt_AvaiCS, out int carSMSec)) errMsg += $"「南車用信号機の進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を\n入力してください。\n";
                if (!CheckSecText(txt_AvaiCE, out int carEMSec)) errMsg += $"「東車用信号機の進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を\n入力してください。\n";
                if (!CheckSecText(txt_AvaiCW, out int carWMSec)) errMsg += $"「西車用信号機の進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を\n入力してください。\n";
                if (!CheckSecText(txt_AvaiPNS, out int pedNSMSec)) errMsg += $"「北南歩行者用信号機の進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を\n入力してください。\n";
                if (!CheckSecText(txt_AvaiPEW, out int pedEWMSec)) errMsg += $"「東西歩行者用信号機の進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を\n入力してください。\n";
                if (!CheckSecText(txt_LightOnArw, out int arwMSec)) errMsg += $"「矢印信号機の点灯時間」には{ArrowSecMin}から{ArrowSecMax}の整数を入力してください。\n";

                if (errMsg == "")
                {
                    SetMSecValues = new TrafficMSecValues(carNMSec, carSMSec, carEMSec, carWMSec, pedNSMSec, pedEWMSec, arwMSec);
                    Close();
                }
                else
                {
                    MessageBox.Show(errMsg, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                string exStr = "「確定」ボタンのクリックでエラーが発生しました。\n" + ex.Message;
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
                if (txtBox == txt_LightOnArw)
                {
                    maxVal = ArrowSecMax;  // 矢印信号機点灯秒数の最大値を取得
                    minVal = ArrowSecMin;  // 矢印信号機点灯秒数の最小値を取得
                }

                if (!double.TryParse(txtBox.Text, out double dbevalue)) return false;  // Textプロパティ値がdouble型に変換できない場合は終了する
                if (!int.TryParse(dbevalue.ToString(), out resultVal))  return false;  // double型から変換した文字列がint型に変換できない場合は終了する
                if (resultVal < minVal || resultVal > maxVal)           return false;  // int型に変換した値がminValより小さい、もしくはmaxValより大きい場合は終了する
                resultVal *= 1000;
                return true;
            }
            catch (Exception ex)
            {
                string exStr = "入力値のチェックでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                resultVal = 0;
                return false;
            }
        }
    }
}
