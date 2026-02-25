using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TrafficLightAlgorithm
{
    public partial class F_TrafficLight : Form
    {
        /// <summary>
        /// 進行可能時間の最大値
        /// </summary>
        private const int ProgSecMax      = 20;

        /// <summary>
        /// 進行可能時間の最小値
        /// </summary>
        private const int ProgSecMin      = 5;

        /// <summary>
        /// 矢印信号機の点灯時間の最大値
        /// </summary>
        private const int ArrowSecMax     = 5;

        /// <summary>
        /// 矢印信号機の点灯時間の最小値
        /// </summary>
        private const int ArrowSecMin     = 1;

        /// <summary>
        /// 全信号機の赤点灯時間の最大値
        /// </summary>
        private const int AllRedSecMax    = 5;

        /// <summary>
        /// 全信号機の赤点灯時間の最小値
        /// </summary>
        private const int AllRedSecMin    = 1;

        /// <summary>
        /// 車用信号機の黄点灯ミリ秒
        /// </summary>
        private const int YellowMSec      = 1000;

        /// <summary>
        /// 車用信号機の点滅間隔ミリ秒
        /// </summary>
        private const int BlinkMSec       = 500;

        /// <summary>
        /// 信号機点灯ミリ秒の最小値
        /// </summary>
        private const int MinMSec         = 1000;

        /// <summary>
        /// 歩行者用信号機点滅の合計フェーズ数
        /// </summary>
        private const int BlinkPhaseCount = 5;

        /// <summary>
        /// 信号機点灯処理の中断時点のフェーズを表す番号
        /// </summary>
        private int InterruptPhase;

        /// <summary>
        /// 信号機アルゴリズムが動いている・中断している場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsTrafficEnable;

        /// <summary>
        /// 信号機アルゴリズムの中断が有効の場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsInterrupt;

        /// <summary>
        /// 信号機アルゴリズムの停止が有効の場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsCancel;

        /// <summary>
        /// 信号機アルゴリズムのフェーズリスト
        /// </summary>
        private List<TrafficPhase> PhaseList;

        public F_TrafficLight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        private void F_TrafficLight_Load(object sender, EventArgs e)
        {
            IsTrafficEnable = false;  // 信号機アルゴリズムが動かない場合のブール値に設定する
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする

            // 東方向矢印信号機の矢印ランプを表すラベルの回転
            lbl_EArrow.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            // 北方向歩行者用信号機のランプを表すラベルの回転
            lbl_PNGreOne.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
            lbl_PNGreTwo.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
            lbl_PNRedOne.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
            lbl_PNRedTwo.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);

            // 南方向歩行者用信号機のランプを表すラベルの回転
            lbl_PSGreOne.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
            lbl_PSGreTwo.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
            lbl_PSRedOne.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
            lbl_PSRedTwo.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);

            // 東方向歩行者用信号機のランプを表すラベルの回転
            lbl_PEGreTwo.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            lbl_PERedTwo.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            // 西方向歩行者用信号機のランプを表すラベルの回転
            lbl_PWGreTwo.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            lbl_PWRedTwo.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            // 西方向車道の進行方向矢印を表示するラベルの回転
            lbl_WSLArrow.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            lbl_WRArrow.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            // 交差点コーナー画像表示ラベルの回転
            lbl_NorthEastCorner.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
            lbl_SouthEastCorner.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            lbl_SouthWestCorner.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
        }

        /// <summary>
        /// 「開始」ボタンクリック時イベント
        /// </summary>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            string errMsg = CreateErrMsg();  // エラーメッセージを取得する

            // エラーメッセージ表示
            if (errMsg != "")
            {
                MessageBox.Show(errMsg, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 信号機アルゴリズムを中断している場合
            if (IsInterrupt)
            {
                string msgStr = "信号機の点灯処理を中断しています。処理を最初から実行しますか？";
                DialogResult diresult = MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (diresult == DialogResult.No) return;
            }
            else
            {
                if (IsTrafficEnable) return;  // 信号機アルゴリズムが中断されず動いている場合は終了する
            }

            // 信号機アルゴリズムの設定値構造体を取得する
            WaitMSec waitMSec = new WaitMSec(txt_NProgSec.Text, txt_SProgSec.Text, txt_EProgSec.Text,
                                             txt_WProgSec.Text, txt_ArrowSec.Text, txt_AllRedSec.Text);
            
            IsTrafficEnable = true;   // 信号機アルゴリズムが動く場合のブール値に設定する
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする       

            ChangeTextInterruptResumeBtn();                // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(false);                   // テキストボックスのEnabledプロパティ値変更
            PhaseList = CreateTrafficPhaseList(waitMSec);  // 信号機アルゴリズムのフェーズリストを作成
            LoopTrafficPhase(0);                     　    // フェーズリストの最初のフェーズからループを開始する
        }

        /// <summary>
        /// 「中断/再開」ボタンクリック時イベント
        /// </summary>
        private void Btn_InterruptResume_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                if (IsInterrupt)
                {
                    IsInterrupt = false;  // 信号機アルゴリズムの中断を無効にする
                }
                else
                {
                    IsInterrupt = true;   // 信号機アルゴリズムの中断を有効にする
                }

                ChangeTextInterruptResumeBtn();  // 「中断/再開」ボタンのTextプロパティ値変更

                if (IsInterrupt)
                {
                    IsCancel = true;  // 信号機アルゴリズムの停止を有効にする
                }
                else
                {
                    IsCancel = false;                  // 信号機アルゴリズムの停止を無効にする
                    LoopTrafficPhase(InterruptPhase);  // 中断したフェーズからループを開始する
                }
            }
        }

        /// <summary>
        /// 「リセット」ボタンクリック時イベント
        /// </summary>
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            if (IsTrafficEnable)
            {
                string msgStr = "信号機の点灯状態をリセットしますか？";
                DialogResult diresult = MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (diresult == DialogResult.No) return;
            }

            IsTrafficEnable = false;  // 信号機アルゴリズムが動かない場合のブール値に設定する
            IsInterrupt     = false;  // 信号機アルゴリズムの中断を無効にする
            IsCancel        = true;   // 信号機アルゴリズムの停止を有効にする

            ChangeTextInterruptResumeBtn();  // 「中断/再開」ボタンのTextプロパティ値変更
            ChangeTextBoxEnabled(true);      // テキストボックスのEnabledプロパティ値変更
            
            ChangeSignalLightOn(LightState.NoLight, lbl_NGreen, lbl_NYellow, lbl_NRed, null);        // 北車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_SGreen, lbl_SYellow, lbl_SRed, null);        // 南車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_EGreen, lbl_EYellow, lbl_ERed, lbl_EArrow);  // 東車用信号機を無灯火にする
            ChangeSignalLightOn(LightState.NoLight, lbl_WGreen, lbl_WYellow, lbl_WRed, lbl_WArrow);  // 西車用信号機を無灯火にする

            ChangePedesLightOn(LightState.NoLight, lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo);  // 北歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight, lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo);  // 南歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight, lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo);  // 東歩行者用信号機を無灯火にする
            ChangePedesLightOn(LightState.NoLight, lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo);  // 西歩行者用信号機を無灯火にする
        }
        
        /// <summary>
        /// 「バージョン情報」ボタンクリック時イベント
        /// </summary>
        private void Btn_VersionShow_Click(object sender, EventArgs e)
        {
            Version ver    = typeof(F_TrafficLight).Assembly.GetName().Version;  // バージョン情報を取得
            string  verstr = $"Ver{ver.Minor}.{ver.Build}{ver.Revision}";        // Ver{マイナーバージョン}.{ビルド番号}{リビジョン}形式の文字列を作成する

            MessageBox.Show(verstr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// エラーメッセージを作成する
        /// </summary>
        /// <returns> エラーメッセージを表す文字列 </returns>
        private string CreateErrMsg()
        {
            string errMsg = "";  // エラーメッセージが入る

            // テキストボックスのTextプロパティ値がチェックを満たさない場合はエラーメッセージを追加する
            if (!CheckSecText(txt_NProgSec.Text,  ProgSecMin,   ProgSecMax))   errMsg += $"「北方向への進行可能時間」には{ProgSecMin}から{ProgSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_SProgSec.Text,  ProgSecMin,   ProgSecMax))   errMsg += $"「南方向への進行可能時間」には{ProgSecMin}から{ProgSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_EProgSec.Text,  ProgSecMin,   ProgSecMax))   errMsg += $"「東方向への進行可能時間」には{ProgSecMin}から{ProgSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_WProgSec.Text,  ProgSecMin,   ProgSecMax))   errMsg += $"「西方向への進行可能時間」には{ProgSecMin}から{ProgSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_ArrowSec.Text,  ArrowSecMin,  ArrowSecMax))  errMsg += $"「矢印信号機の点灯時間」には{ArrowSecMin}から{ArrowSecMax}の整数を入力してください。\n";
            if (!CheckSecText(txt_AllRedSec.Text, AllRedSecMin, AllRedSecMax)) errMsg += $"「全信号機の赤点灯時間」には{AllRedSecMin}から{AllRedSecMax}の整数を入力してください。\n";

            return errMsg;
        }

        /// <summary>
        /// 文字列が最大値と最小値の範囲内の整数を表す値かチェックを行う
        /// </summary>
        /// <param name="checkText"> チェック対象の文字列         </param>
        /// <param name="maxValue">  チェックを満たす整数の最大値 </param>
        /// <param name="minValue">  チェックを満たす整数の最小値 </param>
        /// <returns> checkTextをint型に変換した値がminValue以上でmaxValue以下の整数の場合はtrue、それ以外の場合はfalse </returns>
        private bool CheckSecText(string checkText, int minValue, int maxValue)
        {
            if (!int.TryParse(checkText, out int checkValue))   return false;  // チェック対象の文字列がint型に変換できない場合は終了する
            if (checkValue < minValue || checkValue > maxValue) return false;  // int型に変換した値がminValueより小さい、もしくはmaxValueより大きい場合は終了する
            return true;
        }

        /// <summary>
        /// 「中断/再開」ボタンのtextプロパティ値変更
        /// </summary>
        private void ChangeTextInterruptResumeBtn()
        {
            // IsInterruptがtrueの場合Textプロパティ値は「再開」、それ以外の場合は「中断」に設定する
            if (IsInterrupt) 
            { 
                btn_InterruptResume.Text = "再開";  
            }
            else
            {
                btn_InterruptResume.Text = "中断";
            }
        }

        /// <summary>
        /// テキストボックスのEnabledプロパティ値を変更する
        /// </summary>
        /// <param name="enable"> Enabledプロパティを有効にする場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextBoxEnabled(bool enable)
        {
            txt_NProgSec.Enabled  = enable;  // 交差点北方向への進行可能秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_SProgSec.Enabled  = enable;  // 交差点南方向への進行可能秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_EProgSec.Enabled  = enable;  // 交差点東方向への進行可能秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_WProgSec.Enabled  = enable;  // 交差点西方向への進行可能秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_ArrowSec.Enabled  = enable;  // 矢印信号機の点灯秒数を入力するテキストボックスのEnabledプロパティ値変更
            txt_AllRedSec.Enabled = enable;  // 全信号機の赤点灯秒数を入力するテキストボックスのEnabledプロパティ値変更
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストを作成する
        /// </summary>
        /// <param name="setTraffic"> 信号機の設定値構造体 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CreateTrafficPhaseList(WaitMSec setTraffic)
        {
            List<TrafficPhase> phaseList = new List<TrafficPhase>
            {
                // 全ての車用・歩行者用信号機の赤点灯
                CreatePhase(setTraffic.RMSec, LightState.Red,
                    Traffic.CarNorth,   Traffic.CarSouth,   Traffic.CarEast,   Traffic.CarWest,
                    Traffic.PedesNorth, Traffic.PedesSouth, Traffic.PedesEast, Traffic.PedesWest),

                // 北南の車用・東西の歩行者用信号機の緑点灯
                CreatePhase(Math.Min(setTraffic.NMSec, setTraffic.SMSec) - BlinkMSec * BlinkPhaseCount - MinMSec, LightState.Green,
                    Traffic.CarNorth,  Traffic.CarSouth,
                    Traffic.PedesEast, Traffic.PedesWest)
            };

            // 東西の歩行者用信号機の点滅
            for (int i = 0; i < BlinkPhaseCount; i++)
            {
                if (i % 2 == 0) phaseList.Add(CreatePhase(BlinkMSec, LightState.NoLight, Traffic.PedesEast, Traffic.PedesWest));
                if (i % 2 == 1) phaseList.Add(CreatePhase(BlinkMSec, LightState.Green,   Traffic.PedesEast, Traffic.PedesWest));
            }

            // 東西の歩行者用信号機の赤点灯
            phaseList.Add(CreatePhase(MinMSec, LightState.Red, Traffic.PedesEast, Traffic.PedesWest));

            // 北南の車用信号機の黄・赤点灯
            if (setTraffic.NMSec >= setTraffic.SMSec)
            {
                phaseList.AddRange(CreateYellowRedPhaseList(setTraffic.RMSec, setTraffic.NMSec - setTraffic.SMSec, Traffic.CarSouth, Traffic.CarNorth));
            }
            else
            {
                phaseList.AddRange(CreateYellowRedPhaseList(setTraffic.RMSec, setTraffic.SMSec - setTraffic.NMSec, Traffic.CarNorth, Traffic.CarSouth));
            }

            // 東西の車用・北南の歩行者用信号機の緑点灯
            phaseList.Add(CreatePhase(Math.Min(setTraffic.EMSec, setTraffic.WMSec) - BlinkMSec * BlinkPhaseCount - MinMSec, LightState.Green,
                Traffic.CarEast,    Traffic.CarWest, 
                Traffic.PedesNorth, Traffic.PedesSouth));

            // 北南の歩行者用信号機の点滅
            for (int i = 0; i < BlinkPhaseCount; i++)
            {
                if (i % 2 == 0) phaseList.Add(CreatePhase(BlinkMSec, LightState.NoLight, Traffic.PedesNorth, Traffic.PedesSouth));
                if (i % 2 == 1) phaseList.Add(CreatePhase(BlinkMSec, LightState.Green,   Traffic.PedesNorth, Traffic.PedesSouth));
            }

            // 北南の歩行者用信号機の赤点灯
            phaseList.Add(CreatePhase(MinMSec, LightState.Red, Traffic.PedesNorth, Traffic.PedesSouth));

            // 東西の車用信号機の黄・赤点灯
            if (setTraffic.EMSec >= setTraffic.WMSec)
            {
                phaseList.AddRange(CreateYellowRedPhaseList(MinMSec, setTraffic.EMSec - setTraffic.WMSec, Traffic.CarWest, Traffic.CarEast));
            }
            else
            {
                phaseList.AddRange(CreateYellowRedPhaseList(MinMSec, setTraffic.WMSec - setTraffic.EMSec, Traffic.CarEast, Traffic.CarWest));
            }

            // 東西の矢印信号機の点灯
            phaseList.Add(CreatePhase(setTraffic.AMSec, LightState.Arrow, Traffic.CarEast, Traffic.CarWest));

            // 東西の車用信号機の黄点灯
            phaseList.Add(CreatePhase(YellowMSec, LightState.Yellow, Traffic.CarEast, Traffic.CarWest));  

            return phaseList;
        }

        /// <summary>
        /// 複数の車用・歩行者用信号機を同じ時間・同じ色に点灯するフェーズを作成
        /// </summary>
        /// <param name="mSec">     点灯時間ミリ秒 </param>
        /// <param name="state">    点灯状態       </param>
        /// <param name="traffics"> 点灯する信号機 </param>
        /// <returns> 作成したフェーズ </returns>
        private TrafficPhase CreatePhase(int mSec, LightState state, params Traffic[] traffics)
        {
            TrafficCommand[] commands = new TrafficCommand[traffics.Length];  // 信号機の種類と点灯状態の設定値が入る

            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = new TrafficCommand(traffics[i], state);
            }

            return new TrafficPhase(mSec, commands);
        }

        /// <summary>
        /// 車用信号機の黄・赤点灯フェーズリストを作成する
        /// </summary>
        /// <param name="redMSec"> carMaxが表す車用信号機の赤点灯ミリ秒             </param>
        /// <param name="MSecdif"> carMinとcarMaxが表す車用信号機の緑点灯ミリ秒の差 </param>
        /// <param name="carMin">  緑点灯時間が短い車用信号機を表す列挙型           </param>
        /// <param name="carMax">  緑点灯時間が長い車用信号機を表す列挙型           </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CreateYellowRedPhaseList(int redMSec, int MSecdif, Traffic carMin, Traffic carMax)
        {
            if (MSecdif == 0)
            {
                // carMinとcarMaxが表す車用信号機の緑点灯時間が一致する場合
                return new List<TrafficPhase>
                {
                    CreatePhase(YellowMSec, LightState.Yellow, carMin, carMax),  // carMinとcarMaxが表す車用信号機を黄に点灯
                    CreatePhase(redMSec,    LightState.Red,    carMin, carMax)   // carMinとcarMaxが表す車用信号機を赤に点灯
                };
            }
            else if (MSecdif == MinMSec)
            {
                // MSecdifとMinMSecが等しい場合
                return new List<TrafficPhase>
                {
                    new TrafficPhase(YellowMSec, 
                        new TrafficCommand(carMin, LightState.Yellow)),  // carMinが表す車用信号機を黄に点灯
                    new TrafficPhase(YellowMSec, 
                        new TrafficCommand(carMin, LightState.Red),
                        new TrafficCommand(carMax, LightState.Yellow)),  // carMinが表す車用信号機を赤、carMaxが表す車用信号機を黄に点灯
                    new TrafficPhase(redMSec,
                        new TrafficCommand(carMax, LightState.Red)),     // carMaxが表す車用信号機を赤に点灯
                };
            }

            return new List<TrafficPhase>
            {
                new TrafficPhase(YellowMSec, 
                    new TrafficCommand(carMin, LightState.Yellow)),  // carMinが表す車用信号機を黄に点灯
                new TrafficPhase(MSecdif - YellowMSec,
                    new TrafficCommand(carMin, LightState.Red)),     // carMinが表す車用信号機を赤に点灯
                new TrafficPhase(YellowMSec,
                    new TrafficCommand(carMax, LightState.Yellow)),  // carMaxが表す車用信号機を黄に点灯
                new TrafficPhase(redMSec,
                    new TrafficCommand(carMax, LightState.Red)),     // carMaxが表す車用信号機を赤に点灯
            };
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストをループさせる
        /// </summary>
        /// <param name="phaseNum"> 開始フェーズの番号を表す整数 </param>
        private async void LoopTrafficPhase(int phaseNum)
        {
            int startPhase = phaseNum;  // 開始フェーズの番号を取得する

            while (!IsCancel)
            {
                for (int i = startPhase; i < PhaseList.Count; i++)
                {
                    InterruptPhase = i;  // 現在のフェーズの番号を取得する
                    
                    foreach(TrafficCommand command in PhaseList[i].Commands)
                    {
                        if (command.Traffic == Traffic.CarNorth)
                        {
                            ChangeSignalLightOn(command.State, lbl_NGreen, lbl_NYellow, lbl_NRed, null);  // 北車用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.CarSouth)
                        {
                            ChangeSignalLightOn(command.State, lbl_SGreen, lbl_SYellow, lbl_SRed, null);  // 南車用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.CarEast)
                        {
                            ChangeSignalLightOn(command.State, lbl_EGreen, lbl_EYellow, lbl_ERed, lbl_EArrow);  // 東車用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.CarWest)
                        {
                            ChangeSignalLightOn(command.State, lbl_WGreen, lbl_WYellow, lbl_WRed, lbl_WArrow);  // 西車用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.PedesNorth)
                        {
                            ChangePedesLightOn(command.State, lbl_PNGreOne, lbl_PNGreTwo, lbl_PNRedOne, lbl_PNRedTwo);  // 北歩行者用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.PedesSouth)
                        {
                            ChangePedesLightOn(command.State, lbl_PSGreOne, lbl_PSGreTwo, lbl_PSRedOne, lbl_PSRedTwo);  // 南歩行者用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.PedesEast)
                        {
                            ChangePedesLightOn(command.State, lbl_PEGreOne, lbl_PEGreTwo, lbl_PERedOne, lbl_PERedTwo);  // 東歩行者用信号機の点灯
                        }
                        else if (command.Traffic == Traffic.PedesWest)
                        {
                            ChangePedesLightOn(command.State, lbl_PWGreOne, lbl_PWGreTwo, lbl_PWRedOne, lbl_PWRedTwo);  // 西歩行者用信号機の点灯
                        }
                    }

                    await Task.Delay(PhaseList[i].WaitMSec);  // WaitMSecミリ秒待機する
                    if (IsCancel) break;                      // 信号機アルゴリズムの停止が有効の場合はループから脱出する
                }

                startPhase = 0;  // 開始フェーズの番号を0に設定する
            }

            IsCancel = false;
        }

        /// <summary>
        /// 車用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">      点灯状態を表す列挙型               </param>
        /// <param name="lbl_green">  車用信号機の緑ランプを表すラベル   </param>
        /// <param name="lbl_yellow"> 車用信号機の黄ランプを表すラベル   </param>
        /// <param name="lbl_red">    車用信号機の赤ランプを表すラベル   </param>
        /// <param name="lbl_arrow">  矢印信号機の矢印ランプを表すラベル </param>
        private void ChangeSignalLightOn(LightState state, Label lbl_green, Label lbl_yellow, Label lbl_red, Label lbl_arrow)
        {
            bool greVisible = false;  // lbl_greenのVisibleプロパティに設定する値
            bool yelVisible = false;  // lbl_yellowのVisibleプロパティに設定する値
            bool redVisible = false;  // lbl_redのVisibleプロパティに設定する値
            bool arwVisible = false;  // lbl_arrowのVisibleプロパティに設定する値

            if (state == LightState.Green) greVisible = true;
            if (state == LightState.Yellow) yelVisible = true;
            if (state == LightState.Red || state == LightState.Arrow) redVisible = true;
            if (state == LightState.Arrow) arwVisible = true;

            lbl_green.Visible  = greVisible;                        // lbl_greenの表示/非表示の設定
            lbl_yellow.Visible = yelVisible;                        // lbl_yellowの表示/非表示の設定
            lbl_red.Visible    = redVisible;                        // lbl_redの表示/非表示の設定
            if (lbl_arrow != null) lbl_arrow.Visible = arwVisible;  // lbl_arrowの表示/非表示の設定
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">        点灯状態を表す列挙型                         </param>
        /// <param name="lbl_greenOne"> 歩行者用信号機の緑ランプを表す１つ目のラベル </param>
        /// <param name="lbl_greenTwo"> 歩行者用信号機の緑ランプを表す２つ目のラベル </param>
        /// <param name="lbl_redOne">   歩行者用信号機の赤ランプを表す１つ目のラベル </param>
        /// <param name="lbl_redTwo">   歩行者用信号機の赤ランプを表す２つ目のラベル </param>
        private void ChangePedesLightOn(LightState state, Label lbl_greenOne, Label lbl_greenTwo, Label lbl_redOne, Label lbl_redTwo)
        {
            bool greVisible = false;  // lbl_greenOneとlbl_greenTwoのVisibleプロパティに設定する値
            bool redVisible = false;  // lbl_redOneとlbl_redTwoのVisibleプロパティに設定する値

            if (state == LightState.Green) greVisible = true;
            if (state == LightState.Red)   redVisible = true;

            lbl_greenOne.Visible = greVisible;  // lbl_greenOneの表示/非表示の設定
            lbl_greenTwo.Visible = greVisible;  // lbl_greenTwoの表示/非表示の設定
            lbl_redOne.Visible   = redVisible;  // lbl_redOneの表示/非表示の設定
            lbl_redTwo.Visible   = redVisible;  // lbl_redTwoの表示/非表示の設定
        }
    }
}