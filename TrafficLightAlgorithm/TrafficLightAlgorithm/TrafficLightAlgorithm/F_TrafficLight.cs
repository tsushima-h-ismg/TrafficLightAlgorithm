using System;
using System.Threading;
using System.Windows.Forms;

namespace TrafficLightAlgorithm
{
    public partial class F_TrafficLight : Form
    {
        /// <summary>
        /// 車用信号機の緑色灯火秒数の最大値
        /// </summary>
        private const int BlueLightOnSecMax = 20;

        /// <summary>
        /// 車用信号機の緑色灯火秒数の最小値
        /// </summary>
        private const int BlueLightOnSecMin = 5;

        /// <summary>
        /// 矢印信号機の緑色灯火秒数の最大値
        /// </summary>
        private const int ArrowLightSecMax = 5;

        /// <summary>
        /// 矢印信号機の緑色灯火秒数の最小値
        /// </summary>
        private const int ArrowLightSecMin = 1;

        /// <summary>
        /// 交差点の進行方向切り替え準備秒数の最大値
        /// </summary>
        private const int PrepareSecMax = 5;

        /// <summary>
        /// 交差点の進行方向切り替え準備秒数の最小値
        /// </summary>
        private const int PrepareSecMin = 1;

        /// <summary>
        /// 車用信号機の黄色灯火時間
        /// </summary>
        private const int YellowSec = 1;

        /// <summary>
        /// 秒数のチェックを満たす最大値と最小値からなる配列
        /// </summary>
        private readonly int[,] SecMaxMinArr =
        {
            { BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, BlueLightOnSecMin, ArrowLightSecMin, PrepareSecMin},
            { BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, BlueLightOnSecMax, ArrowLightSecMax, PrepareSecMax}
        };

        /// <summary>
        /// フォーム画面に入力した数値が入る配列
        /// </summary>
        private int[] InputArr;

        /// <summary>
        /// 信号機の点灯処理を中断した時刻
        /// </summary>
        private DateTime InterruptTime;

        /// <summary>
        /// フォーム画面の「開始」ボタンクリックでtrue、「終了」ボタンクリックもしくはフォームロード時でfalse
        /// </summary>
        private bool IsTrafficEnable;

        /// <summary>
        /// 信号機点灯処理を中断している場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsInterrupt;

        /// <summary>
        /// 北方向車用信号機
        /// </summary>
        private CarTraffic NorthLight;

        /// <summary>
        /// 南方向車用信号機
        /// </summary>
        private CarTraffic SouthLight;
        
        /// <summary>
        /// 東方向車用信号機
        /// </summary>
        private CarTraffic EastLight;
        
        /// <summary>
        /// 西方向車用信号機
        /// </summary>
        private CarTraffic WestLight;

        /// <summary>
        /// 北方向歩行者用信号機
        /// </summary>
        private PedesTraffic NorthPedesLight;

        /// <summary>
        /// 南方向歩行者用信号機
        /// </summary>
        private PedesTraffic SouthPedesLight;

        /// <summary>
        /// 東方向歩行者用信号機
        /// </summary>
        private PedesTraffic EastPedesLight;

        /// <summary>
        /// 西方向歩行者用信号機
        /// </summary>
        private PedesTraffic WestPedesLight;

        /// <summary>
        /// 車用信号機イメージラベルが入る配列
        /// </summary>
        private Label[,] CarLabelArr;

        /// <summary>
        /// 歩行者用信号機イメージラベルが入る配列
        /// </summary>
        private Label[,] PedesLabelArr;

        public F_TrafficLight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        private void F_TrafficLight_Load(object sender, EventArgs e)
        {
            IsInterrupt     = false;
            IsTrafficEnable = false;

            CarLabelArr = new Label[,]{
                { lbl_NorthGreen, lbl_NorthYellow, lbl_NorthRed, null },
                { lbl_SouthGreen, lbl_SouthYellow, lbl_SouthRed, null },
                { lbl_EastGreen,  lbl_EastYellow,  lbl_EastRed,  lbl_EastArrow },
                { lbl_WestGreen,  lbl_WestYellow,  lbl_WestRed,  lbl_WestArrow }
            };

            PedesLabelArr = new Label[,] {
                { lbl_NorthPedesGreenOne, lbl_NorthPedesGreenTwo, lbl_NorthPedesRedOne, lbl_NorthPedesRedTwo },
                { lbl_SouthPedesGreenOne, lbl_SouthPedesGreenTwo, lbl_SouthPedesRedOne, lbl_SouthPedesRedTwo },
                { lbl_EastPedesGreenOne,  lbl_EastPedesGreenTwo,  lbl_EastPedesRedOne,  lbl_EastPedesRedTwo },
                { lbl_WestPedesGreenOne,  lbl_WestPedesGreenTwo,  lbl_WestPedesRedOne,  lbl_WestPedesRedTwo }
            };
        }

        /// <summary>
        /// 「開始」ボタンクリック時イベント
        /// </summary>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            string inputErrStr = CreateErrMsg();  // エラーメッセージを取得する

            // エラーメッセージ表示
            if (inputErrStr != "")
            {
                MessageBox.Show(inputErrStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 信号機点灯処理を中断している場合
            if (IsInterrupt)
            {
                string errMsg = "信号機の点灯処理を中断しています。処理を最初から実行しますか？";
                DialogResult dialogResult = MessageBox.Show(errMsg, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.No) return;
            }

            IsTrafficEnable = true;
            IsInterrupt     = false;
            
            // 現在時刻のミリ秒が100から200の間の秒数になるまでスレッドを中断する
            while (DateTime.Now.Millisecond < 100 || DateTime.Now.Millisecond > 200)
            {
                Thread.Sleep(10);
            }

            InputArr = new int[tlp_InputSecField.RowCount - 1];

            // 車用信号機・矢印信号機の青色灯火時間と進行方向切り替え準備時間を配列に入れる
            for (int i = 0; i < InputArr.Length; i++)
            {
                int.TryParse(tlp_InputSecField.GetControlFromPosition(1, i + 1).Text, out int inputValue);
                InputArr[i] = inputValue;
            }

            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnable(false);                 // テキストボックスのEnabledプロパティ値変更
            InitializeCarLight();                       // 車用信号機を初期状態に設定する
            InitializePedesLight();                     // 歩行者用信号機を初期状態に設定する
            Timer_Traffic.Start();
        }

        /// <summary>
        /// 「中断/再開」ボタンクリック時イベント
        /// </summary>
        private void Btn_InterruptResume_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                // 開始ボタンが押されて終了ボタンが押されていない間はIsInterruptのtrue/falseを切り替える
                if (IsInterrupt)
                {
                    IsInterrupt = false;
                }
                else
                {
                    IsInterrupt = true;
                }
            }
            else
            {
                return;
            }

            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更

            if (IsInterrupt)
            {
                InterruptTime = DateTime.Now;  // 中断時の時刻を取得する
                Timer_Traffic.Stop();
            }
            else
            {
                // 現在時刻のミリ秒が100から200の間の秒数になるまでスレッドを中断する
                while (DateTime.Now.Millisecond < 100 || DateTime.Now.Millisecond > 200)
                {
                    Thread.Sleep(10);
                }

                ResumeCarLight();       // 車用信号機を再開状態に設定する
                ResumePedesLight();     // 歩行者用信号機を再開状態に設定する
                Timer_Traffic.Start();
            }
        }

        /// <summary>
        /// 「終了」ボタンクリック時イベント
        /// </summary>
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                DialogResult = MessageBox.Show("信号機の点灯処理を終了しますか？", Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogResult == DialogResult.No) return;
            }

            // 車用信号機の点灯状態を無灯火に設定する
            NorthLight.UpdateStateNoLight();
            SouthLight.UpdateStateNoLight();
            EastLight.UpdateStateNoLight();
            WestLight.UpdateStateNoLight();

            // 歩行者用信号機の点灯状態を無灯火に設定する
            NorthPedesLight.UpdateStateNoLight();
            SouthPedesLight.UpdateStateNoLight();
            EastPedesLight.UpdateStateNoLight();
            WestPedesLight.UpdateStateNoLight();

            // 車用信号機をイメージしたラベルの背景色を変更する
            ChangeTrafficLblColor(NorthLight);
            ChangeTrafficLblColor(SouthLight);
            ChangeTrafficLblColor(EastLight);
            ChangeTrafficLblColor(WestLight);

            // 歩行者用信号機をイメージしたラベルの背景色を変更する
            ChangePedesLblColor(NorthPedesLight);
            ChangePedesLblColor(SouthPedesLight);
            ChangePedesLblColor(EastPedesLight);
            ChangePedesLblColor(WestPedesLight);

            IsTrafficEnable = false;
            IsInterrupt     = false;
            ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnable(true);                  // 灯火時間と切り替え準備時間テキストボックスのEnabledプロパティ値変更
            Timer_Traffic.Stop();
        }

        /// <summary>
        /// タイマーTickイベント
        /// </summary>
        private void Timer_Traffic_Tick(object sender, EventArgs e)
        {
            ControlTrafficLight();
        }

        /// <summary>
        /// エラーメッセージを作成する
        /// </summary>
        /// <returns> エラーメッセージを表す文字列 </returns>
        private string CreateErrMsg()
        {
            string errStr = "";  // エラーメッセージが入る
            
            // テキストボックスに入力された文字列のチェック
            for (int i = 0; i < tlp_InputSecField.RowCount - 1; i++)
            {
                if (!CheckSecText(tlp_InputSecField.GetControlFromPosition(1, i + 1).Text, SecMaxMinArr[1, i], SecMaxMinArr[0, i]))
                {
                    errStr += $"「{tlp_InputSecField.GetControlFromPosition(0, i + 1).Text}」には{SecMaxMinArr[0, i]}から{SecMaxMinArr[1, i]}の整数を入力してください。\n";
                }
            }

            return errStr;
        }

        /// <summary>
        /// 文字列が最大値と最小値の範囲内の整数を表す値かチェックを行う
        /// </summary>
        /// <param name="checkText"> チェック対象の文字列         </param>
        /// <param name="maxValue">  チェックを満たす整数の最大値 </param>
        /// <param name="minValue">  チェックを満たす整数の最小値 </param>
        /// <returns> checkTextをint型に変換した値がmaxValue以下でminValue以上の整数の場合はtrue。それ以外の場合はfalse </returns>
        private bool CheckSecText(string checkText, int maxValue, int minValue)
        {
            if (!int.TryParse(checkText, out int checkValue))   return false;  // チェック対象の文字列がint型に変換できない場合は終了する
            if (checkValue > maxValue || checkValue < minValue) return false;  // int型に変換した値がmaxValueより大きい、もしくはminValueより小さい場合は終了する
            return true;
        }

        /// <summary>
        /// 「中断/再開」ボタンのtextプロパティ値変更
        /// </summary>
        /// <param name="isInterrupt"> 信号機点灯処理が中断している場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextInterruptResumeBtn(bool isInterrupt)
        {
            if (isInterrupt) 
            { 
                btn_InterruptResume.Text = "再開";            
            }
            else
            {
                btn_InterruptResume.Text = "中断";
            }
        }

        /// <summary>
        /// テキストボックスのEnabledプロパティの設定を変更する
        /// </summary>
        /// <param name="enable"> Enabledプロパティを有効にする場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextBoxEnable(bool enable)
        {
            txt_NLightOnSec.Enabled = enable;
            txt_SLightOnSec.Enabled = enable;
            txt_ELightOnSec.Enabled = enable;
            txt_WLightOnSec.Enabled = enable;
            txt_ArrowSec.Enabled    = enable;
            txt_Prepare.Enabled     = enable;
        }

        /// <summary>
        /// 車用信号機の初期状態を設定
        /// </summary>
        private void InitializeCarLight()
        {
            // 車用信号機クラスのインスタンス生成
            NorthLight = new CarTraffic(CreateTrafficSettingList(0), "Green");
            SouthLight = new CarTraffic(CreateTrafficSettingList(1), "Green");
            EastLight  = new CarTraffic(CreateTrafficSettingList(2), "Red");
            WestLight  = new CarTraffic(CreateTrafficSettingList(3), "Red");

            // 車用信号機を初期の点灯状態に設定する
            ChangeTrafficLblColor(NorthLight);
            ChangeTrafficLblColor(SouthLight);
            ChangeTrafficLblColor(EastLight);
            ChangeTrafficLblColor(WestLight);
        }

        /// <summary>
        /// 車用信号機の設定表を作成する
        /// </summary>
        /// <param name="num"></param>
        /// <returns> 車用信号機の設定表を表すint型配列 </returns>
        private int[] CreateTrafficSettingList(int num)
        {
            int[] trafficSet = {0, 0, 0, 0, 0, 0, 0};

            int indnum1 = num;
            if (num % 2 == 0) indnum1++;
            if (num % 2 == 1) indnum1--;

            int indnum2 = 0;
            if (num < 2) indnum2 = 2;

            trafficSet[0] = num;            // 車用信号機に割り振る番号が入る
            trafficSet[1] = InputArr[num];  // 車用信号機の緑色点灯時間
            trafficSet[2] = YellowSec;      // 車用信号機の黄色点灯時間
            
            if (CarLabelArr[num, 3] != null)
            {
                trafficSet[3] = Math.Max(0, InputArr[indnum1] - InputArr[num]) + 1;  // 車用信号機１回目の赤色点灯時間
                trafficSet[5] = InputArr[4];                                         // 矢印信号機の点灯時間
            }

            // 車用信号機２回目の赤色点灯時間算出
            if (CarLabelArr[indnum2, 3] != null || CarLabelArr[indnum2 + 1, 3] != null)
            {
                trafficSet[4] = Math.Max(InputArr[indnum2], InputArr[indnum2 + 1]) + Math.Max(0, InputArr[indnum1] - InputArr[num]) + YellowSec * 2 + InputArr[5] * 2 + InputArr[4] + 1;
            }
            else
            {
                if (CarLabelArr[num, 3] == null)
                {
                    trafficSet[4] = Math.Max(InputArr[indnum2], InputArr[indnum2 + 1]) + Math.Max(0, InputArr[indnum1] - InputArr[num]) + YellowSec + InputArr[5] * 2;
                }
                else if (CarLabelArr[num, 3] != null)
                {
                    trafficSet[4] = Math.Max(InputArr[indnum2], InputArr[indnum2 + 1]) + YellowSec + InputArr[5] * 2;
                }
            }

            // 点灯処理をずらす秒数
            if (CarLabelArr[num, 3] != null)
            { 
                trafficSet[6] = -InputArr[5];
            }
            else if (CarLabelArr[num, 3] == null)
            {
                trafficSet[6] = -InputArr[5] + Math.Min(0, InputArr[num] - InputArr[indnum1]);
            }

            return trafficSet;
        }

        /// <summary>
        /// 歩行者用信号機の初期状態を設定
        /// </summary>
        private void InitializePedesLight()
        {
            int ns_CarGreenSec = Math.Min(InputArr[0], InputArr[1]);  // 北と南の車用信号機が同時に緑色に点灯する時間を取得
            int ew_CarGreenSec = Math.Min(InputArr[2], InputArr[3]);  // 東と西の車用信号機が同時に緑色に点灯する時間を取得

            // 東方向と西方向の歩行者用信号機の点灯処理をずらす秒数
            int minusSec = -Math.Min(InputArr[0], InputArr[1]) - Math.Abs(InputArr[2] - InputArr[3]) - 1;   

            // 歩行者用信号機のインスタンス生成
            NorthPedesLight = new PedesTraffic(0, ew_CarGreenSec, EastLight.SecCount()  - ew_CarGreenSec + 1, "Red",   DateTime.Now.AddSeconds(minusSec));
            SouthPedesLight = new PedesTraffic(1, ew_CarGreenSec, EastLight.SecCount()  - ew_CarGreenSec + 1, "Red",   DateTime.Now.AddSeconds(minusSec));
            EastPedesLight  = new PedesTraffic(2, ns_CarGreenSec, NorthLight.SecCount() - ns_CarGreenSec + 1, "Green", DateTime.Now);
            WestPedesLight  = new PedesTraffic(3, ns_CarGreenSec, NorthLight.SecCount() - ns_CarGreenSec + 1, "Green", DateTime.Now);

            // 歩行者用信号機を初期の点灯状態に設定する
            ChangePedesLblColor(NorthPedesLight);
            ChangePedesLblColor(SouthPedesLight);
            ChangePedesLblColor(EastPedesLight);
            ChangePedesLblColor(WestPedesLight);
        }

        /// <summary>
        /// 車用信号機の再開状態を設定
        /// </summary>
        private void ResumeCarLight()
        {
            NorthLight.UpdateStateChangeResumeTime(InterruptTime);  // 北方向車用信号機の点灯状態変更時刻を更新する
            SouthLight.UpdateStateChangeResumeTime(InterruptTime);  // 南方向車用信号機の点灯状態変更時刻を更新する
            EastLight.UpdateStateChangeResumeTime(InterruptTime);   // 東方向車用信号機の点灯状態変更時刻を更新する
            WestLight.UpdateStateChangeResumeTime(InterruptTime);   // 西方向車用信号機の点灯状態変更時刻を更新する
        }

        /// <summary>
        /// 歩行者用信号機の再開状態を設定
        /// </summary>
        private void ResumePedesLight()
        {
            NorthPedesLight.UpdateStateChangeResumeTime(InterruptTime);  // 北方向歩行者用信号機の点灯状態変更時刻を更新する
            SouthPedesLight.UpdateStateChangeResumeTime(InterruptTime);  // 南方向歩行者用信号機の点灯状態変更時刻を更新する
            EastPedesLight.UpdateStateChangeResumeTime(InterruptTime);   // 東方向歩行者用信号機の点灯状態変更時刻を更新する
            WestPedesLight.UpdateStateChangeResumeTime(InterruptTime);   // 西方向歩行者用信号機の点灯状態変更時刻を更新する
        }

        /// <summary>
        /// 信号機点灯制御
        /// </summary>
        private void ControlTrafficLight()
        {
            // 車用信号機の点灯状態を更新するか判定し、判定を満たせば点灯状態を更新する
            if (NorthLight.JudgeTrafficLightOn()) UpdateLightOnState(NorthLight);
            if (SouthLight.JudgeTrafficLightOn()) UpdateLightOnState(SouthLight);
            if (EastLight.JudgeTrafficLightOn())  UpdateLightOnState(EastLight);
            if (WestLight.JudgeTrafficLightOn())  UpdateLightOnState(WestLight);

            // 歩行者用信号機の点灯状態を更新するか判定し、判定を満たせば点灯状態を更新する
            if (NorthPedesLight.JudgePedesLightOn()) UpdatePedesLightOnState(NorthPedesLight);
            if (SouthPedesLight.JudgePedesLightOn()) UpdatePedesLightOnState(SouthPedesLight);
            if (EastPedesLight.JudgePedesLightOn())  UpdatePedesLightOnState(EastPedesLight);
            if (WestPedesLight.JudgePedesLightOn())  UpdatePedesLightOnState(WestPedesLight);
        }

        /// <summary>
        /// 車用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="carTraffic"></param>
        private void UpdateLightOnState(CarTraffic carTraffic)
        {
            carTraffic.UpdateLightOnState();    // 車用信号機の点灯状態を更新する
            ChangeTrafficLblColor(carTraffic);  // 点灯状態に合わせてラベルの背景色、フォント色を変更する
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="pedesTraffic"></param>
        private void UpdatePedesLightOnState(PedesTraffic pedesTraffic)
        {
            pedesTraffic.UpdateLightOnState();  // 歩行者用信号機の点灯状態を更新する
            ChangePedesLblColor(pedesTraffic);  // 点灯状態に合わせてラベルの背景色、フォント色を変更する
        }

        /// <summary>
        /// 車用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="carTraffic"> 車用信号機を表すクラス </param>
        private void ChangeTrafficLblColor(CarTraffic carTraffic)
        {
            CarLabelArr[carTraffic.CarTrafficNum, 0].BackColor = carTraffic.LightOnColor()[0];  // 車用信号機の緑ランプを表すラベルの背景色を変更する
            CarLabelArr[carTraffic.CarTrafficNum, 1].BackColor = carTraffic.LightOnColor()[1];  // 車用信号機の黄ランプを表すラベルの背景色を変更する 
            CarLabelArr[carTraffic.CarTrafficNum, 2].BackColor = carTraffic.LightOnColor()[2];  // 車用信号機の赤ランプを表すラベルの背景色を変更する
            
            if (CarLabelArr[carTraffic.CarTrafficNum, 3] != null)
            { 
                CarLabelArr[carTraffic.CarTrafficNum, 3].ForeColor = carTraffic.LightOnColor()[3];  // 矢印信号機を表すラベルのフォント色を変更する
            }
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態を更新する
        /// </summary>
        /// <param name="pedesTraffic"> 歩行者用信号機を表すクラス </param>
        private void ChangePedesLblColor(PedesTraffic pedesTraffic)
        {
            PedesLabelArr[pedesTraffic.PedesNum, 0].BackColor = pedesTraffic.LightOnColor()[0];  // 歩行者用信号機の１つ目の緑ランプを表すラベルの背景色を変更する
            PedesLabelArr[pedesTraffic.PedesNum, 1].BackColor = pedesTraffic.LightOnColor()[1];  // 歩行者用信号機の２つ目の緑ランプを表すラベルの背景色を変更する 
            PedesLabelArr[pedesTraffic.PedesNum, 2].BackColor = pedesTraffic.LightOnColor()[2];  // 歩行者用信号機の１つ目の赤ランプを表すラベルの背景色を変更する 
            PedesLabelArr[pedesTraffic.PedesNum, 3].BackColor = pedesTraffic.LightOnColor()[3];  // 歩行者用信号機の２つ目の赤ランプを表すラベルの背景色を変更する
        }
    }
}