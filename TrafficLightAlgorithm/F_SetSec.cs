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
        /// 設定値の変更が有効の場合はtrue、それ以外の場合はfalse
        /// </summary>
        public bool IsEnable;

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
                bool arrowEnable = IsEnable;          // 矢印信号機点灯秒数を入力するテキストボックスのEnabledプロパティ値
                if (IsEnable) arrowEnable = IsArrow;  // IsEnableがtrueの場合に矢印信号機が存在するか判定する

                string oppositeDirStr = "北";  // SetSignalの反対方向を表す文字列
                if      (SetDirection == Direction.North) oppositeDirStr = CreateDirStr(Direction.South);
                else if (SetDirection == Direction.East)  oppositeDirStr = CreateDirStr(Direction.West);
                else if (SetDirection == Direction.West)  oppositeDirStr = CreateDirStr(Direction.East);

                lbl_FormTitle.Text  = CreateDirStr(SetDirection) + CreateSigStr(SetSignal) + "信号機の設定値";  // フォームタイトルを取得

                txt_AvaiSec.Text    = AvaiSec.ToString();  // 進行可能秒数を取得
                txt_AvaiSec.Enabled = IsEnable;            // 進行可能秒数入力欄テキストボックスのEnabledプロパティ値設定

                if (SetSignal == Signal.Pedes)
                {
                    lbl_SupplementAvai.Text = "※" + oppositeDirStr + "方向の" + CreateSigStr(SetSignal) + "信号機の点灯時間と共通設定";
                }

                if (IsArrow)
                {
                    txt_ArrowSec.Text    = ArrowSec.ToString();  // 矢印信号機点灯秒数を取得
                    txt_ArrowSec.Enabled = arrowEnable;          // 矢印信号機点灯秒数テキストボックスのEnabledプロパティ値設定

                    lbl_SupplementArrow.Text = "※" + oppositeDirStr + "方向の矢印信号機の点灯時間と共通設定";
                }
                else if (!IsArrow)
                {
                    // 矢印信号機が存在しない場合、項目名と入力欄を非表示にする
                    lbl_ArrowSec.Visible        = false;
                    txt_ArrowSec.Visible        = false;
                    lbl_SecTwo.Visible          = false;
                    lbl_SupplementArrow.Visible = false;

                    btn_Confirm.Top -= (txt_ArrowSec.Location.Y - txt_AvaiSec.Location.Y);  // 確定ボタンを上に詰める
                    Height          -= (txt_ArrowSec.Location.Y - txt_AvaiSec.Location.Y);  // フォーム画面を上に詰める
                }
            }
            catch (Exception ex)
            {
                string exStr = "設定値入力フォームのロードでエラーが発生しました。\n" + ex.Message;
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 方角を表す列挙型を文字列に変換
        /// </summary>
        /// <param name="direction"> 方角を表す列挙型 </param>
        /// <returns> 変換後の文字列 </returns>
        private string CreateDirStr(Direction direction)
        {
            try
            {
                if      (direction == Direction.North) return "北";
                else if (direction == Direction.South) return "南";
                else if (direction == Direction.East)  return "東";
                else if (direction == Direction.West)  return "西";
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 信号機の種類を表す列挙型を文字列に変換
        /// </summary>
        /// <param name="signal"> 信号機の種類を表す列挙型 </param>
        /// <returns> 変換後の文字列 </returns>
        private string CreateSigStr(Signal signal)
        {
            try
            {
                if      (signal == Signal.Car)   return "車用";
                else if (signal == Signal.Pedes) return "歩行者用";
                return "";
            }
            catch
            {
                return "";
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
                if (!CheckSecText(txt_AvaiSec,  out int avaiVal))             errMsg += $"「進行可能時間」には{AvaiSecMin}から{AvaiSecMax}の整数を入力してください。\n";
                if (!CheckSecText(txt_ArrowSec, out int arrowVal) && IsArrow) errMsg += $"「矢印信号機の点灯時間」には{ArrowSecMin}から{ArrowSecMax}の整数を入力してください。\n";

                if (errMsg == "")
                {
                    AvaiSec  = avaiVal;                // テキストボックスに入力した進行可能秒数を取得する
                    if (IsArrow) ArrowSec = arrowVal;  // テキストボックスに入力した矢印信号機の点灯秒数を取得する
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