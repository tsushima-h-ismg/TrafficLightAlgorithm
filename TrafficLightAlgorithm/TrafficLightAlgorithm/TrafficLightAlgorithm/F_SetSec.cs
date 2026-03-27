using System;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_SetSec : Form
    {
        /// <summary>
        /// 設定値の名称
        /// </summary>
        public string SetValueName { get; set; }

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
        /// 信号機アルゴリズムを実行している状態でフォームが呼び出された場合はtrue、それ以外の場合はfalse
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 進行可能秒数の最大値
        /// </summary>
        private const int AvaiSecMax = 20;

        /// <summary>
        /// 進行可能秒数の最小値
        /// </summary>
        private const int AvaiSecMin = 5;

        /// <summary>
        /// 矢印信号機の点灯秒数の最大値
        /// </summary>
        private const int ArrowSecMax = 5;

        /// <summary>
        /// 矢印信号機の点灯秒数の最小値
        /// </summary>
        private const int ArrowSecMin = 1;

        /// <summary>
        /// 全信号機の赤点灯秒数の最大値
        /// </summary>
        private const int AllRedSecMax = 5;

        /// <summary>
        /// 全信号機の赤点灯秒数の最小値
        /// </summary>
        private const int AllRedSecMin = 1;

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
                lbl_SetValueName.Text = SetValueName + "：";  // 設定値の名称を取得
                txt_SetValue.Text     = SetValue.ToString();  // 設定値を取得
                txt_SetValue.Enabled  = IsEnable;             // 信号機アルゴリズムが実行されている場合はtrue、それ以外の場合はfalse

                txt_ArrowSec.Text = ArrowSec.ToString();  // 矢印信号機の点灯秒を取得

                if (IsEnable)
                {
                    txt_ArrowSec.Enabled = IsArrow;  // 矢印信号機が存在する場合はtrue、それ以外の場合はfalse
                }
                else
                {
                    txt_ArrowSec.Enabled = false;
                }

                txt_RedSec.Text = AllRedSec.ToString();  // 全信号機の赤点灯秒を取得
                txt_RedSec.Enabled = IsEnable;           // 信号機アルゴリズムが実行されている場合はtrue、それ以外の場合はfalse
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nフォームのロードに失敗しました。";
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
                string exStr = ex.Message + "\n入力値の確定に失敗しました。";
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
                if (!double.TryParse(checktext, out double douvalue))       return false;  // チェック対象文字列がdouble型に変換できない場合は終了する
                if (!int.TryParse(douvalue.ToString(), out int checkValue)) return false;  // double型から変換した文字列がint型に変換できない場合は終了する
                if (checkValue < minValue || checkValue > maxValue)         return false;  // int型に変換した値がminValueより小さい、もしくはmaxValueより大きい場合は終了する
                return true;
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n文字列のチェックに失敗しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
