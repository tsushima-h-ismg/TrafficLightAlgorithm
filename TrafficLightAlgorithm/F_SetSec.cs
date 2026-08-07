using System;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_SetSec : Form
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
        /// 進行可能秒数
        /// </summary>
        public int AvaiSec;

        /// <summary>
        /// 矢印信号機の点灯秒数
        /// </summary>
        public int ArrowSec;

        /// <summary>
        /// 矢印信号機を持つ場合はtrue、それ以外の場合はfalse
        /// </summary>
        public bool IsArrow;

        /// <summary>
        /// 信号機を設置した方角
        /// </summary>
        public Direction SetDirection;

        /// <summary>
        /// 信号機の種類
        /// </summary>
        public Signal SetSignal;

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
                lbl_FormTitle.Text = CreateDirStr(SetDirection) + CreateSigStr(SetSignal) + "信号機の設定値";

                string oppositeDirStr = "北";  // SetDirectionの反対方向を表す文字列

                if (SetDirection == Direction.North) oppositeDirStr = CreateDirStr(Direction.South);
                else if (SetDirection == Direction.East) oppositeDirStr = CreateDirStr(Direction.West);
                else if (SetDirection == Direction.West) oppositeDirStr = CreateDirStr(Direction.East);

                txt_AvaiSec.Text = AvaiSec.ToString();  // 進行可能秒数

                if (SetSignal == Signal.Pedes)
                {
                    lbl_SupplementAvai.Text = "※" + oppositeDirStr + "方向の" + CreateSigStr(SetSignal) + "信号機の進行可能時間と共通設定";
                }

                if (IsArrow)
                {
                    txt_ArrowSec.Text = ArrowSec.ToString();  // 矢印信号機の点灯秒数

                    lbl_SupplementArrow.Text = "※" + oppositeDirStr + "方向の矢印信号機の点灯時間と共通設定";
                }
                else
                {
                    // 矢印信号機が存在しない場合、項目名と入力欄を非表示にする
                    lbl_ArrowSec.Visible = false;
                    txt_ArrowSec.Visible = false;
                    lbl_SecTwo.Visible = false;
                    lbl_SupplementArrow.Visible = false;

                    btn_Confirm.Top -= txt_ArrowSec.Location.Y - txt_AvaiSec.Location.Y;  // 確定ボタンを上に詰める
                    Height          -= txt_ArrowSec.Location.Y - txt_AvaiSec.Location.Y;  // フォーム画面を上に詰める
                }
            }
            catch (Exception ex)
            {
                string exStr = "設定値入力画面の読み込みでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 信号機の設置方角を表す列挙型を文字列に変換
        /// </summary>
        /// <param name="direction"> 信号機設置方角 </param>
        /// <returns> 変換後の文字列 </returns>
        private string CreateDirStr(Direction direction)
        {
            if      (direction == Direction.North) return "北";
            else if (direction == Direction.South) return "南";
            else if (direction == Direction.East)  return "東";
            else if (direction == Direction.West)  return "西";
            return "";
        }

        /// <summary>
        /// 信号機の種類を表す列挙型を文字列に変換
        /// </summary>
        /// <param name="signal"> 信号機の種類 </param>
        /// <returns> 変換後の文字列 </returns>
        private string CreateSigStr(Signal signal)
        {
            if (signal == Signal.Car) return "車用";
            else if (signal == Signal.Pedes) return "歩行者用";
            return "";
        }

        /// <summary>
        /// 確定ボタンクリック時イベント
        /// </summary>
        private void Btn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                int arrowVal = ArrowSec;  // 矢印信号機の点灯秒数
                string errMsg = "";

                // 進行可能秒数の入力チェック
                if (!CheckSecText(txt_AvaiSec, out int avaiVal)) errMsg += $"「進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";

                // 矢印信号機の点灯秒数の入力チェック
                if (IsArrow)
                {
                    if (!CheckSecText(txt_ArrowSec, out arrowVal)) errMsg += $"「矢印信号機の点灯時間」には{ArrowSecMin}から{ArrowSecMax}の整数を入力してください。\n";
                }

                if (errMsg == "")
                {
                    AvaiSec = avaiVal;
                    ArrowSec = arrowVal;
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
        /// テキストボックスのTextプロパティ値のチェック
        /// </summary>
        /// <param name="txtBox">    チェック対象のテキストボックス                  </param>
        /// <param name="resultVal"> 引数txtBoxのTextプロパティ値をint型に変換した値 </param>
        /// <returns> 引数txtBoxのTextプロパティ値をint型に変換した値が最大値と最小値の範囲を満たす場合はtrue、それ以外ではfalse </returns>
        private bool CheckSecText(TextBox txtBox, out int resultVal)
        {
            try
            {
                resultVal = 0;

                int maxVal = AvaiSecMax;  // チェックを満たす最大値
                int minVal = AvaiSecMin;  // チェックを満たす最小値

                // 矢印信号機点灯秒数の入力欄をチェックする場合
                if (txtBox == txt_ArrowSec)
                {
                    maxVal = ArrowSecMax;
                    minVal = ArrowSecMin;
                }

                if (!double.TryParse(txtBox.Text, out double dbevalue)) return false;
                if (!int.TryParse(dbevalue.ToString(), out resultVal))  return false;  // 「1.00」や「3.0」といった文字列を通すためのチェック
                if (resultVal < minVal || resultVal > maxVal)           return false;  // 最大値と最小値の範囲を満たすかチェック
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