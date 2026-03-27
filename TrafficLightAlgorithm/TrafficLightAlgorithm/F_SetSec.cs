using System;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_SetSec : Form
    {
        /// <summary>
        /// 設定値
        /// </summary>
        public int SetValue { get; set; }

        /// <summary>
        /// 矢印信号機点灯秒
        /// </summary>
        public int ArrowSec { get; set; }

        /// <summary>
        /// 全信号機の赤点灯秒
        /// </summary>
        public int AllRedSec { get; set; }

        /// <summary>
        /// 矢印信号機が存在する場合はtrue、それ以外の場合はfalse
        /// </summary>
        public bool IsArrow { get; set; }

        /// <summary>
        /// 信号機アルゴリズムが動いていない状態でフォームが呼び出された場合はtrue、それ以外の場合はfalse
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 方角を表す文字列
        /// </summary>
        public string DirectionName { get; set; }

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
        /// 全信号機の赤点灯秒数の最大値
        /// </summary>
        private const int AllRedSecMax = 5;

        /// <summary>
        /// 全信号機の赤点灯秒数の最小値
        /// </summary>
        private const int AllRedSecMin = 1;

        /// <summary>
        /// 設定値の名称
        /// </summary>
        private string SetValueName;

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

                SetValueName = DirectionName + "方向への進行可能時間";

                lbl_SoftTitle.Text    = DirectionName + "信号機の設定値入力";  // フォームタイトルを取得
                lbl_SetValueName.Text = SetValueName  + "：";                  // 設定値の名称を取得
              
                txt_SetValue.Text    = SetValue.ToString();  // 設定値入力欄テキストボックスのTextプロパティ値設定
                txt_SetValue.Enabled = IsEnable;             // 設定値入力欄テキストボックスのEnabledプロパティ値設定

                txt_ArrowSec.Text    = ArrowSec.ToString();  // 矢印信号機点灯秒数テキストボックスのTextプロパティ値設定
                txt_ArrowSec.Enabled = arrowEnable;          // 矢印信号機点灯秒数テキストボックスのEnabledプロパティ値設定

                txt_RedSec.Text    = AllRedSec.ToString();  // 全信号機赤点灯秒数テキストボックスのTextプロパティ値設定
                txt_RedSec.Enabled = IsEnable;              // 全信号機赤点灯秒数テキストボックスのEnabledプロパティ値設定
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n設定値入力フォームのロードでエラーが発生しました。";
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
                if (!CheckSecText(txt_SetValue.Text, AvaiSecMin,   AvaiSecMax))   errMsg += $"「{SetValueName}」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
                if (!CheckSecText(txt_ArrowSec.Text, ArrowSecMin,  ArrowSecMax))  errMsg += $"「矢印信号機の点灯時間」には{ArrowSecMin}から{ArrowSecMax}の整数を入力してください。\n";
                if (!CheckSecText(txt_RedSec.Text,   AllRedSecMin, AllRedSecMax)) errMsg += $"「全信号機の赤点灯時間」には{AllRedSecMin}から{AllRedSecMax}の整数を入力してください。\n";

                // エラーメッセージ表示
                if (errMsg != "")
                {
                    MessageBox.Show(errMsg, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SetValue  = ConvertToInt(txt_SetValue.Text);  // 設定値の値を取得する
                ArrowSec  = ConvertToInt(txt_ArrowSec.Text);  // 矢印信号機の点灯秒数を取得する
                AllRedSec = ConvertToInt(txt_RedSec.Text);    // 全信号機の赤点灯秒を取得する
                Close();
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n「確定」ボタンクリックでエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 文字列が最大値と最小値の範囲内の整数を表す値かチェックを行う
        /// </summary>
        /// <param name="checktext"> チェック対象の文字列         </param>
        /// <param name="minValue">  チェックを満たす整数の最小値 </param> 
        /// <param name="maxValue">  チェックを満たす整数の最大値 </param>
        /// <returns> checkTextをint型に変換した値がminValue以上でmaxValue以下の整数の場合はtrue、それ以外の場合はfalse </returns>
        private bool CheckSecText(string checktext, int minValue, int maxValue)
        {
            try
            {
                if (!double.TryParse(checktext, out double dbevalue))       return false;  // チェック対象文字列がdouble型に変換できない場合は終了する
                if (!int.TryParse(dbevalue.ToString(), out int checkValue)) return false;  // double型から変換した文字列がint型に変換できない場合は終了する
                if (checkValue < minValue || checkValue > maxValue)         return false;  // int型に変換した値がminValueより小さい、もしくはmaxValueより大きい場合は終了する
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列を整数に変換
        /// </summary>
        /// <param name="str"> 変換元の文字列 </param>
        /// <returns> 変換後の整数 </returns>
        private int ConvertToInt(string str)
        {
            try
            {
                double.TryParse(str, out double douValue);          // 変換元の文字列をdouble型に変換
                int.TryParse(douValue.ToString(), out int result);  // double型から変換した文字列をint型に変換
                return result;
            }
            catch
            {
                return 0;
            }
        }
    }
}
