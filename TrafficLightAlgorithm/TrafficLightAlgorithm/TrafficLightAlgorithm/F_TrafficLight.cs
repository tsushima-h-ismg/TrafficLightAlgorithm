using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TrafficLightAlgorithm
{
    public partial class F_TrafficLight : Form
    {
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
        /// 全信号機の赤点灯ミリ秒
        /// </summary>
        private const int AllRedMSec      = 1000;

        /// <summary>
        /// 歩行者用信号機点滅の合計フェーズ数
        /// </summary>
        private const int BlinkPhaseCount = 5;

        /// <summary>
        /// 信号機アルゴリズムのフェーズ再生で中断したフェーズを表す番号
        /// </summary>
        private int InterruptPhase;

        /// <summary>
        /// 点灯状態変更履歴のリストから参照する要素の番号
        /// </summary>
        private int LogEleNum;

        /// <summary>
        /// 信号機アルゴリズムが動いている・中断している場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsTrafficEnable;

        /// <summary>
        /// 信号機アルゴリズムの中断が有効の場合はtrue、それ以外の場合はfalse
        /// </summary>
        private bool IsInterrupt;

        /// <summary>
        /// キャンセル要求
        /// </summary>
        private CancellationTokenSource Cts;

        /// <summary>
        /// 信号機アルゴリズムのフェーズリスト
        /// </summary>
        private List<TrafficPhase> PhaseList;

        /// <summary>
        /// 信号機の点灯状態変更履歴
        /// </summary>
        private List<string> LightChangeLog;

        /// <summary>
        /// 設定値構造体
        /// </summary>
        private WaitMSec SetMSec;

        public F_TrafficLight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        private void F_TrafficLight_Load(object sender, EventArgs e)
        {
            try
            {
                IsTrafficEnable = false;              // 信号機アルゴリズムが動かない場合のブール値に設定する
                IsInterrupt     = false;              // 信号機アルゴリズムの中断を無効にする
                Cts = new CancellationTokenSource();  // Ctsの初期化

                //ピクチャボックスイメージの回転
                RotateLabelImage(RotateFlipType.Rotate90FlipNone,  pib_PNGreOne, pib_PNRedOne,    pib_PSGreOne, pib_PSRedOne, pib_PSSignalTwo, pib_PNSignalTwo);
                RotateLabelImage(RotateFlipType.Rotate180FlipNone, pib_EArrow,   pib_PEGreTwo,    pib_PERedTwo, pib_PWGreTwo, pib_PWRedTwo,    pib_SSignal,
                                                                   pib_ESignal,  pib_PESignalTwo, pib_PWSignalTwo);
                RotateLabelImage(RotateFlipType.Rotate270FlipNone, pib_PNGreTwo, pib_PNRedTwo,    pib_PSGreTwo, pib_PSRedTwo, pib_PNSignalOne, pib_PSSignalOne);

                SetMSec = new WaitMSec(5000, 5000, 5000, 5000, 1000);  // 信号機点灯時間の初期設定

                //ピクチャボックスのコントロールコレクションにラベルを追加する
                pib_NSignal.Controls.Add(lbl_NSignal);
                pib_SSignal.Controls.Add(lbl_SSignal);
                pib_ESignal.Controls.AddRange(new Control[] { lbl_ESignal, lbl_EArrow });
                pib_WSignal.Controls.AddRange(new Control[] { lbl_WSignal, lbl_WArrow });
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nフォームのロードでエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// ピクチャボックスのイメージを回転する
        /// </summary>
        /// <param name="rotate"> イメージの回転量と反転軸           </param>
        /// <param name="pibs">   イメージ回転を行うピクチャボックス </param>
        private void RotateLabelImage(RotateFlipType rotate, params PictureBox[] pibs)
        {
            try
            {
                foreach (PictureBox pib in pibs)
                {
                    pib.BackgroundImage.RotateFlip(rotate);  // ピクチャボックスの背景画像を回転する
                }
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nピクチャボックスイメージの回転でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 信号機イメージ画像クリック時イベント
        /// </summary>
        private void Lbl_CarSignal_Click(object sender, EventArgs e)
        {
            try
            {
                // 設定値入力フォームを表示し、値を受け取る
                if (sender == lbl_NSignal || sender == pib_NGreen || sender == pib_NYellow || sender == pib_NRed)
                {
                    SetFormShow(SetMSec.NMSec, SetMSec.AMSec, "北", false, pib_NSignal);  // 北車用信号機の設定値入力
                }
                else if (sender == lbl_SSignal || sender == pib_SGreen || sender == pib_SYellow || sender == pib_SRed)
                {
                    SetFormShow(SetMSec.SMSec, SetMSec.AMSec, "南", false, pib_SSignal);  // 南車用信号機の設定値入力
                }
                else if (sender == lbl_ESignal || sender == lbl_EArrow || sender == pib_EGreen || sender == pib_EYellow || sender == pib_ERed || sender == pib_EArrow)
                {
                    SetFormShow(SetMSec.EMSec, SetMSec.AMSec, "東", true,  pib_ESignal);  // 東車用信号機の設定値入力
                }
                else if (sender == lbl_WSignal || sender == lbl_WArrow || sender == pib_WGreen || sender == pib_WYellow || sender == pib_WRed || sender == pib_WArrow)
                {
                    SetFormShow(SetMSec.WMSec, SetMSec.AMSec, "西", true,  pib_WSignal);  // 西車用信号機の設定値入力
                }
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n設定値入力フォームの表示でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 設定値入力フォームを表示する
        /// </summary>
        /// <param name="avaiMSec"> 進行可能ミリ秒数                                </param>
        /// <param name="arrMSec">  矢印信号機の点灯ミリ秒数                        </param>
        /// <param name="dir">      方角を表す文字列                                </param>
        /// <param name="isArrow">  trueで矢印信号機を有する、それ以外の場合はfalse </param>
        /// <param name="pib">      背景画像の強調表示を行うピクチャボックス        </param>
        private void SetFormShow(int avaiMSec, int arrMSec, string dir, bool isArrow, PictureBox pib)
        {
            try
            {
                int clientleft = (Width  - ClientSize.Width) / 2;                              // フォーム左端からクライアント領域左端までの横幅
                int clientTop  = Height - ClientSize.Height - (Width - ClientSize.Width) / 2;  // フォーム上端からクライアント領域上端までの縦幅

                // 交差点イメージ図の左上端の座標
                Point pnlTopLeft = new Point(Location.X + clientleft + grb_TrafficShow.Location.X + pnl_Traffic.Location.X,
                                             Location.Y + clientTop  + grb_TrafficShow.Location.Y + pnl_Traffic.Location.Y);
                
                int xlocation    = pnlTopLeft.X + pib.Location.X + pib.Width;  // フォームの初期表示位置のx座標
                int ylocation    = pnlTopLeft.Y + pib.Location.Y;              // フォームの初期表示位置のy座標
                Rectangle scArea = Screen.FromPoint(pnlTopLeft).WorkingArea;   // 交差点イメージ図を表示するスクリーンの作業領域を取得

                Image backImage     = pib.BackgroundImage;       // 強調表示前の背景画像を取得
                Image hilightImage  = SignalHilight(backImage);  // 強調表示した背景画像を取得
                if (hilightImage == null) return;                // 強調表示した画像が取得できなかった場合は終了する
                pib.BackgroundImage = hilightImage;              // ピクチャボックスの背景画像を強調表示した画像にする

                // 設定値入力フォームを初期化
                F_SetSec f_SetSec = new F_SetSec
                { 
                    AvaiSec       = avaiMSec / 1000,
                    ArrowSec      = arrMSec  / 1000,
                    DirectionName = dir,
                    IsArrow       = isArrow,
                    IsEnable      = !IsTrafficEnable
                };
                
                // フォームの初期位置がスクリーン内に収めるように初期位置を設定する
                if (xlocation > scArea.Right  - f_SetSec.Width)  xlocation = scArea.Right  - f_SetSec.Width;
                if (ylocation > scArea.Bottom - f_SetSec.Height) ylocation = scArea.Bottom - f_SetSec.Height; 
                f_SetSec.Location = new Point(xlocation, ylocation);

                f_SetSec.ShowDialog();  // 設定値入力フォームを表示

                int avaimsec = f_SetSec.AvaiSec  * 1000;  // 設定値入力フォームから進行可能秒数を取得
                int arrmsec  = f_SetSec.ArrowSec * 1000;  // 設定値入力フォームから矢印信号機点灯秒数を取得

                // 設定値構造体を初期化
                if (pib == pib_NSignal) SetMSec = new WaitMSec(avaimsec,      SetMSec.SMSec, SetMSec.EMSec, SetMSec.WMSec, arrmsec);
                if (pib == pib_SSignal) SetMSec = new WaitMSec(SetMSec.NMSec, avaimsec,      SetMSec.EMSec, SetMSec.WMSec, arrmsec);
                if (pib == pib_ESignal) SetMSec = new WaitMSec(SetMSec.NMSec, SetMSec.SMSec, avaimsec,      SetMSec.WMSec, arrmsec);
                if (pib == pib_WSignal) SetMSec = new WaitMSec(SetMSec.NMSec, SetMSec.SMSec, SetMSec.EMSec, avaimsec,      arrmsec);

                pib.BackgroundImage = backImage;  // ピクチャボックスの背景画像を元に戻す
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 信号機イメージ画像を強調表示する
        /// </summary>
        /// <param name="img"> 強調表示する元の画像 </param>
        /// <returns> 強調表示を行った後の画像 </returns>
        private Image SignalHilight(Image img)
        {
            try
            {
                Bitmap bmp = new Bitmap(img.Width, img.Height);  // 元の画像と同じサイズのビットマップを取得

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // カラーマトリックスを表す配列を作成
                    float[][] hilightArray = { new float[] { 2, 0, 0, 0, 0 },
                                               new float[] { 0, 2, 0, 0, 0 },
                                               new float[] { 0, 0, 2, 0, 0 },
                                               new float[] { 0, 0, 0, 2, 0 },
                                               new float[] { 0, 0, 0, 0, 2 } };
                    ImageAttributes ia = new ImageAttributes();
                    ColorMatrix cm = new ColorMatrix(hilightArray);  // カラーマトリックスを作成
                    ia.SetColorMatrix(cm);                           // カラー調整行列を設定

                    // 引数imgと同じサイズで強調表示した画像を作成
                    g.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                }

                return bmp;
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n信号機イメージ画像の強調表示でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// 「開始」ボタンクリック時イベント
        /// </summary>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            try
            {
                // 信号機アルゴリズムが動いている場合
                if (IsTrafficEnable)
                {
                    string msgStr = "信号機プログラムを最初から実行しますか？";
                    if (MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;

                    Cts.Cancel();  // 先に実行した信号機アルゴリズムのキャンセル要求を伝える
                }

                LogEleNum = 0;                                // 参照するリストの要素の番号を０に設定する
                ChangeTextInterruptResumeBtn(false);          // 「中断/再開」ボタンのTextプロパティ値変更
                PhaseList = CreateTrafficPhaseList(SetMSec);  // フェーズリスト作成
                
                if (PhaseList != null)
                {
                    IsTrafficEnable = true;          // 信号機アルゴリズムが動く場合のブール値に設定する
                    IsInterrupt     = false;         // 信号機アルゴリズムの中断を無効にする
                    LoopTrafficPhase(0, PhaseList);  // フェーズリストを最初のフェーズから再生する
                }
            }
            catch (Exception ex) 
            {
                string exStr = ex.Message + "\n信号機の点灯を開始する前にエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 「中断/再開」ボタンクリック時イベント
        /// </summary>
        private void Btn_InterruptResume_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsTrafficEnable) return;  // 信号機アルゴリズムが動いていない場合は終了する
                
                if (IsInterrupt)
                {
                    IsInterrupt = false;                          // 信号機アルゴリズムの中断を無効にする
                    LoopTrafficPhase(InterruptPhase, PhaseList);  // 中断したフェーズからフェーズリストを再生する
                }
                else
                {
                    IsInterrupt = true;  // 信号機アルゴリズムの中断を有効にする
                    Cts.Cancel();        // キャンセル要求を伝える
                }
                
                ChangeTextInterruptResumeBtn(IsInterrupt);  // 「中断/再開」ボタンのTextプロパティ値変更
            }
            catch (Exception ex)
            {
                string irtext = "再開";
                if (IsInterrupt) irtext = "中断";
                string exStr = ex.Message + "\n信号機プログラムの" + irtext + "でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 「リセット」ボタンクリック時イベント
        /// </summary>
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTrafficEnable)
                {
                    string msgStr = "信号機の点灯状態をリセットしますか？";
                    if (MessageBox.Show(msgStr, Program.SoftTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return; 
                }

                Cts.Cancel();  // キャンセル要求を伝える
                
                ChangeSignalLightOn(LightState.NoLight, pib_NGreen,   pib_NYellow,  pib_NRed,     null);          // 北方向の車用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, pib_SGreen,   pib_SYellow,  pib_SRed,     null);          // 南方向の車用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, pib_EGreen,   pib_EYellow,  pib_ERed,     pib_EArrow);    // 東方向の車用信号機を無灯火にする
                ChangeSignalLightOn(LightState.NoLight, pib_WGreen,   pib_WYellow,  pib_WRed,     pib_WArrow);    // 西方向の車用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  pib_PNGreOne, pib_PNGreTwo, pib_PNRedOne, pib_PNRedTwo);  // 北方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  pib_PSGreOne, pib_PSGreTwo, pib_PSRedOne, pib_PSRedTwo);  // 南方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  pib_PEGreOne, pib_PEGreTwo, pib_PERedOne, pib_PERedTwo);  // 東方向の歩行者用信号機を無灯火にする
                ChangePedesLightOn(LightState.NoLight,  pib_PWGreOne, pib_PWGreTwo, pib_PWRedOne, pib_PWRedTwo);  // 西方向の歩行者用信号機を無灯火にする
                
                IsTrafficEnable = false;              // 信号機アルゴリズムが動かない場合のブール値に設定する
                IsInterrupt     = false;              // 信号機アルゴリズムの中断を無効にする
                lbx_StateRecord.Items.Clear();        // 点灯状態変更履歴をクリアにする
                ChangeTextInterruptResumeBtn(false);  // 「中断/再開」ボタンのTextプロパティ値変更
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n信号機の点灯状態のリセットでエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// タイトル表示ラベルクリック時イベント
        /// </summary>
        private void Lbl_SoftTitle_Click(object sender, EventArgs e)
        {
            try
            {
                F_Version f_Version = new F_Version();  // バージョン情報フォームの初期化
                f_Version.ShowDialog();                 // バージョン情報フォーム表示
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\nバージョン情報フォーム画面の表示でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        /// <summary>
        /// 「中断/再開」ボタンのtextプロパティ値変更
        /// </summary>
        /// <param name="isInterrupt"> 信号機アルゴリズムの中断が有効の場合はtrue、それ以外の場合はfalse </param>
        private void ChangeTextInterruptResumeBtn(bool isInterrupt)
        {
            try
            {
                if (isInterrupt) 
                { 
                    btn_InterruptResume.Text = "再開";  // 信号機アルゴリズムの中断が有効の場合は「再開」に設定する
                }
                else
                {
                    btn_InterruptResume.Text = "中断";  // 信号機アルゴリズムの中断が無効の場合は「中断」に設定する
                }
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n「中断/再開」ボタンテキストの変更でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストを作成する
        /// </summary>
        /// <param name="setMSec"> 信号機のミリ秒設定値構造体 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> CreateTrafficPhaseList(WaitMSec setMSec)
        {
            try
            {
                int carNSpdEWMSec = Math.Min(setMSec.NMSec, setMSec.SMSec) - BlinkMSec * BlinkPhaseCount - MinMSec;  // 北南の車用・東西の歩行者用信号機が緑に同時点灯するミリ秒数
                int carEWpdNSMSec = Math.Min(setMSec.EMSec, setMSec.WMSec) - BlinkMSec * BlinkPhaseCount - MinMSec;  // 東西の車用・北南の歩行者用信号機が緑に同時点灯するミリ秒数

                LightChangeLog = new List<string>();
                List<TrafficPhase> phaseList = new List<TrafficPhase>
                {
                    CreatePhase(AllRedMSec, LightState.Red, 
                        Signal.CarNorth,   Signal.CarSouth,   Signal.CarEast,   Signal.CarWest, 
                        Signal.PedesNorth, Signal.PedesSouth, Signal.PedesEast, Signal.PedesWest),  // 全ての車用・歩行者用信号機の赤点灯フェーズ
                    
                    CreatePhase(carNSpdEWMSec, LightState.Green, 
                        Signal.CarNorth,   Signal.CarSouth,   Signal.PedesEast, Signal.PedesWest)   // 北南の車用・東西の歩行者用信号機の緑点灯フェーズ 
                };

                phaseList.AddRange(PedesBlink(Signal.PedesEast, Signal.PedesWest));                        // 東西の歩行者用信号機の点滅フェーズリスト
                phaseList.Add(CreatePhase(MinMSec, LightState.Red, Signal.PedesEast,  Signal.PedesWest));  // 東西の歩行者用信号機の赤点灯フェーズ
                
                phaseList.AddRange(YelRedPhaseList(AllRedMSec, setMSec.NMSec - setMSec.SMSec, Signal.CarNorth, Signal.CarSouth));  // 北南の車用信号機の黄・赤点灯フェーズリスト

                phaseList.Add(CreatePhase(carEWpdNSMSec, LightState.Green, 
                    Signal.CarEast, Signal.CarWest, Signal.PedesNorth, Signal.PedesSouth));  // 東西の車用・北南の歩行者用信号機の緑点灯フェーズ

                phaseList.AddRange(PedesBlink(Signal.PedesNorth, Signal.PedesSouth));                       // 北南の歩行者用信号機の点滅フェーズリスト
                phaseList.Add(CreatePhase(MinMSec, LightState.Red, Signal.PedesNorth, Signal.PedesSouth));  // 北南の歩行者用信号機の赤点灯フェーズ

                phaseList.AddRange(YelRedPhaseList(MinMSec, setMSec.EMSec - setMSec.WMSec, Signal.CarEast, Signal.CarWest));  // 東西の車用信号機の黄・赤点灯フェーズリスト

                phaseList.Add(CreatePhase(setMSec.AMSec, LightState.Arrow,  Signal.CarEast, Signal.CarWest));  // 東西の矢印信号機の点灯フェーズ
                phaseList.Add(CreatePhase(YellowMSec,    LightState.Yellow, Signal.CarEast, Signal.CarWest));  // 東西の車用信号機の黄点灯フェーズ
                return phaseList;
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n信号機点灯フェーズリストの作成でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>
        /// 複数の車用・歩行者用信号機を同じ時間待機・同じ色に点灯するフェーズを作成
        /// </summary>
        /// <param name="mSec">    待機時間ミリ秒 </param>
        /// <param name="state">   点灯状態       </param>
        /// <param name="signals"> 点灯する信号機 </param>
        /// <returns> 作成したフェーズ </returns>
        private TrafficPhase CreatePhase(int mSec, LightState state, params Signal[] signals)
        {
            try
            {
                TrafficCommand[] commands = new TrafficCommand[signals.Length];

                for (int i = 0; i < commands.Length; i++)
                {
                    commands[i] = new TrafficCommand(signals[i], state);  // 信号機を表す列挙型と点灯状態を表す列挙型が入る
                }

                LightChangeLog.Add(SigStateStr(mSec, state, new List<Signal>(signals)));  // 点灯状態変更履歴を追加
                return new TrafficPhase(mSec, commands);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 歩行者用信号機の点滅フェーズリスト作成
        /// </summary>
        /// <param name="pSigOne"> １つ目の歩行者用信号機を表す列挙型 </param>
        /// <param name="pSigTwo"> ２つ目の歩行者用信号機を表す列挙型 </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> PedesBlink(Signal pSigOne, Signal pSigTwo)
        {
            try
            {
                List<TrafficPhase> phaseList = new List<TrafficPhase>();  // 歩行者用信号機点滅フェーズのリスト
                
                for (int i = 0; i < BlinkPhaseCount; i++)
                {
                    if (i % 2 == 0)
                    {
                        phaseList.Add(CreatePhase(BlinkMSec, LightState.NoLight, pSigOne, pSigTwo));  // 歩行者用信号機を無灯火にするフェーズ
                    }
                    else
                    {
                        phaseList.Add(CreatePhase(BlinkMSec, LightState.Green,   pSigOne, pSigTwo));  // 歩行者用信号機を緑に点灯するフェーズ
                    }
                }

                LightChangeLog.RemoveRange(LightChangeLog.Count - BlinkPhaseCount, BlinkPhaseCount - 1);
                return phaseList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 車用信号機の黄・赤点灯フェーズリストを作成する
        /// </summary>
        /// <param name="redMSec"> sigTwoが表す車用信号機の赤点灯ミリ秒             </param>
        /// <param name="mSecDif"> sigOneとsigTwoが表す車用信号機の緑点灯ミリ秒の差 </param>
        /// <param name="sigOne">  １つ目の車用信号機を表す列挙型                   </param>
        /// <param name="sigTwo">  ２つ目の車用信号機を表す列挙型                   </param>
        /// <returns> 作成したフェーズリスト </returns>
        private List<TrafficPhase> YelRedPhaseList(int redMSec, int mSecDif, Signal sigOne, Signal sigTwo)
        {
            try
            {
                Signal sigMin = sigOne;                               // 進行可能時間が短い方の車用信号機を表す列挙型が入る
                Signal sigMax = sigTwo;                               // 進行可能時間が長い方の車用信号機を表す列挙型が入る
                List<TrafficPhase> pList = new List<TrafficPhase>();  // sigOneとsigTwoが表す車用信号機の黄・赤点灯フェーズが入るフェーズリスト
                
                if (mSecDif < 0)
                {
                    sigMin = sigTwo;
                    sigMax = sigOne;
                }

                if (mSecDif == 0)
                {
                    // sigMinとsigMaxが表す車用信号機の進行可能ミリ秒数が一致する場合
                    pList.Add(CreatePhase(YellowMSec, LightState.Yellow, sigMin, sigMax));  // sigMinとsigMaxが表す車用信号機の黄点灯
                    pList.Add(CreatePhase(redMSec,    LightState.Red,    sigMin, sigMax));  // sigMinとsigMaxが表す車用信号機の赤点灯
                }
                else if (Math.Abs(mSecDif) == YellowMSec)
                {
                    // sigMinとsigMaxが表す車用信号機の進行可能ミリ秒数の差がYellowMSecと一致する場合
                    pList.Add(new TrafficPhase(YellowMSec, new TrafficCommand(sigMin, LightState.Yellow)));  // sigMinが表す車用信号機の黄点灯
                    pList.Add(new TrafficPhase(YellowMSec, new TrafficCommand(sigMin, LightState.Red),
                                                           new TrafficCommand(sigMax, LightState.Yellow)));  // sigMinが表す車用信号機の赤点灯、sigMaxが表す車用信号機の黄点灯
                    pList.Add(new TrafficPhase(redMSec,    new TrafficCommand(sigMax, LightState.Red)));     // sigMaxが表す車用信号機の赤点灯
                    LightChangeLog.AddRange(new List<string> { YellowMSec + "ミリ秒点灯。" + SigStr(sigMin) + "が黄に点灯しました。" ,
                                                               YellowMSec + "ミリ秒点灯。" + SigStr(sigMin) + "が赤・" + SigStr(sigMax) + "が黄に点灯しました。",
                                                               redMSec    + "ミリ秒点灯。" + SigStr(sigMax) + "が赤に点灯しました。"});
                }
                else
                {
                    pList.Add(new TrafficPhase(YellowMSec,                     new TrafficCommand(sigMin, LightState.Yellow)));  // sigMinが表す車用信号機の黄点灯
                    pList.Add(new TrafficPhase(Math.Abs(mSecDif) - YellowMSec, new TrafficCommand(sigMin, LightState.Red)));     // sigMinが表す車用信号機の赤点灯
                    pList.Add(new TrafficPhase(YellowMSec,                     new TrafficCommand(sigMax, LightState.Yellow)));  // sigMaxが表す車用信号機の黄点灯
                    pList.Add(new TrafficPhase(redMSec,                        new TrafficCommand(sigMax, LightState.Red)));     // sigMaxが表す車用信号機の赤点灯
                    LightChangeLog.AddRange(new List<string> { YellowMSec + "ミリ秒点灯。" + SigStr(sigMin) + "が黄に点灯しました。" ,
                                           Math.Abs(mSecDif) - YellowMSec + "ミリ秒点灯。" + SigStr(sigMax) + "が赤に点灯しました。",
                                                               YellowMSec + "ミリ秒点灯。" + SigStr(sigMin) + "が黄に点灯しました。",
                                                               redMSec    + "ミリ秒点灯。" + SigStr(sigMax) + "が赤に点灯しました。"});
                }

                return pList;
            }
            catch
            {
                throw;
            }
        }

        private string SigStateStr(int mSec, LightState state, List<Signal> siglist)
        {
            try
            {
                string result = "";

                if (siglist[0] == Signal.CarNorth && siglist[siglist.Count - 1] == Signal.PedesWest && state == LightState.Red)   result = "全信号機";
                if (siglist[0] == Signal.CarNorth && siglist[siglist.Count - 1] == Signal.PedesWest && state == LightState.Green) result = "北南車用・東西歩行者用信号機";
                if (siglist[0] == Signal.CarEast  && siglist[siglist.Count - 1] == Signal.PedesSouth)                             result = "東西車用・北南歩行者用信号機";

                if (siglist[siglist.Count - 1] - siglist[0] == 1) result = SigStr(siglist[0], siglist[1]);
                if (siglist[0] == siglist[siglist.Count - 1])     result = SigStr(siglist[0]);              // siglistの要素数が１の場合

                if (result != "" && state == LightState.Green)  result = mSec + "ミリ秒点灯。" + result + "が緑に点灯しました。";
                if (result != "" && state == LightState.Yellow) result = mSec + "ミリ秒点灯。" + result + "が黄に点灯しました。";
                if (result != "" && state == LightState.Red)    result = mSec + "ミリ秒点灯。" + result + "が赤に点灯しました。";
                if (mSec == BlinkMSec)   result = BlinkPhaseCount * BlinkMSec + "ミリ秒点灯。" + result + "が点滅しました。";
                
                if (result != "" && state == LightState.Arrow) result = mSec + "ミリ秒点灯。東西の矢印信号機が点灯しました。";
                
                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 信号機列挙型から文字列を返す
        /// </summary>
        /// <param name="signals"> 信号機を表す列挙型 </param>
        /// <returns> 信号機を表す文字列 </returns>
        private string SigStr(params Signal[] signals)
        {
            try
            {
                if (signals.Length == 1 && (signals[0] == Signal.CarNorth || signals[0] == Signal.PedesNorth)) return "北車用信号機";
                if (signals.Length == 1 && (signals[0] == Signal.CarSouth || signals[0] == Signal.PedesSouth)) return "南車用信号機";
                if (signals.Length == 1 && (signals[0] == Signal.CarEast  || signals[0] == Signal.PedesEast))  return "東車用信号機";
                if (signals.Length == 1 && (signals[0] == Signal.CarWest  || signals[0] == Signal.PedesWest))  return "西車用信号機";
                if (signals[0] == Signal.CarNorth   && signals[1] == Signal.CarSouth)   return "北南車用信号機";
                if (signals[0] == Signal.CarEast    && signals[1] == Signal.CarWest)    return "東西車用信号機";
                if (signals[0] == Signal.PedesNorth && signals[1] == Signal.PedesSouth) return "北南歩行者用信号機";
                if (signals[0] == Signal.PedesEast  && signals[1] == Signal.PedesWest)  return "東西歩行者用信号機";
                if (signals.Length == 8 && signals[0] == Signal.CarNorth && signals[signals.Length - 1] == Signal.PedesWest) return "全信号機";
                if (signals.Length == 4 && signals[0] == Signal.CarNorth && signals[signals.Length - 1] == Signal.PedesWest) return "北南車用・東西歩行者用信号機";
                if (signals.Length == 4 && signals[signals.Length - 1] == Signal.PedesSouth) return "東西車用・北南歩行者用信号機"; 
                return "";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 信号機アルゴリズムのフェーズリストをループさせる
        /// </summary>
        /// <param name="phaseNum"> 最初に再生するフェーズを表す番号 </param>
        /// <param name="phases">   ループ再生するフェーズリスト     </param>
        private async void LoopTrafficPhase(int phaseNum, List<TrafficPhase> phases)
        {
            try
            {
                int startPhase = phaseNum;            // 最初に再生するフェーズの番号を取得する
                Cts = new CancellationTokenSource();  // Ctsの初期化
                
                while (!Cts.IsCancellationRequested)
                {
                    for (int i = startPhase; i < phases.Count; i++)
                    {
                        InterruptPhase = i;  // 現在のフェーズを表す番号を取得する

                        foreach (TrafficCommand command in phases[i].Commands)
                        {
                            // 点灯状態の更新でfalseが戻り値の場合は終了する
                            if (command.Signal == Signal.CarNorth)
                            {
                                if (!ChangeSignalLightOn(command.State, pib_NGreen, pib_NYellow, pib_NRed, null)) return;  // 北方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.CarSouth)
                            {
                                if (!ChangeSignalLightOn(command.State, pib_SGreen, pib_SYellow, pib_SRed, null)) return;  // 南方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.CarEast)
                            {
                                if (!ChangeSignalLightOn(command.State, pib_EGreen, pib_EYellow, pib_ERed, pib_EArrow)) return;  // 東方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.CarWest)
                            {
                                if (!ChangeSignalLightOn(command.State, pib_WGreen, pib_WYellow, pib_WRed, pib_WArrow)) return;  // 西方向の車用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesNorth)
                            {
                                if (!ChangePedesLightOn(command.State, pib_PNGreOne, pib_PNGreTwo, pib_PNRedOne, pib_PNRedTwo)) return;  // 北方向の歩行者用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesSouth)
                            {
                                if (!ChangePedesLightOn(command.State, pib_PSGreOne, pib_PSGreTwo, pib_PSRedOne, pib_PSRedTwo)) return;  // 南方向の歩行者用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesEast)
                            {
                                if (!ChangePedesLightOn(command.State, pib_PEGreOne, pib_PEGreTwo, pib_PERedOne, pib_PERedTwo)) return;  // 東方向の歩行者用信号機の点灯状態更新
                            }
                            else if (command.Signal == Signal.PedesWest)
                            {
                                if (!ChangePedesLightOn(command.State, pib_PWGreOne, pib_PWGreTwo, pib_PWRedOne, pib_PWRedTwo)) return;  // 西方向の歩行者用信号機の点灯状態更新
                            }
                        }

                        try
                        {
                            if (!CreateStateRecord(i, phases)) return;        // 点灯状態変更履歴の追加に失敗した場合は終了する
                            await Task.Delay(phases[i].WaitMSec, Cts.Token);  // キャンセルが要求されていない場合、WaitMSecミリ秒待機する
                        }
                        catch
                        {
                            LogEleNum--;
                            return;  // 待機中に例外が発生した場合は終了する
                        }
                    }
                    
                    startPhase = 0;  // 次のループで最初から再生するフェーズの番号を0に設定する
                }
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n信号機の点灯でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// リストボックスに状態変更履歴を表す文字列を追加する
        /// </summary>
        /// <param name="phaseNum"> 信号機アルゴリズムの点灯フェーズを表す番号 </param>
        /// <param name="phases">   信号機アルゴリズムの点灯フェーズリスト     </param>
        /// <returns> 状態変更履歴の追加に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool CreateStateRecord(int phaseNum, List<TrafficPhase> phases)
        {
            try
            {
                // 点灯フェーズphases[phaseNum]の待機ミリ秒がBlinkMSecより大きい、もしくは最初に点滅を行うフェーズの場合、点灯状態変更内容をリストボックスに追加する
                if (phases[phaseNum].WaitMSec  >  BlinkMSec || 
                    (phases[phaseNum].WaitMSec == BlinkMSec && phases[phaseNum - 1].WaitMSec > BlinkMSec))
                {
                    lbx_StateRecord.Items.Add(lbx_StateRecord.Items.Count + "：" + LightChangeLog[LogEleNum]);  // 点灯状態変更内容をリストボックスに追加する
                    LogEleNum++;                                                                                // 次に参照する要素の番号を指定する
                    if (LogEleNum >= LightChangeLog.Count) LogEleNum = 0;                                       // LogEleNumがリストの項目数以上の場合０にする
                    lbx_StateRecord.TopIndex = lbx_StateRecord.Items.Count - 1;                                 // 最新の履歴を表示する
                }

                return true;
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n信号機の点灯状態変更履歴の追加でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 車用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">      点灯状態を表す列挙型                       </param>
        /// <param name="pib_green">  車用信号機の緑ランプを表すピクチャボックス </param>
        /// <param name="pib_yellow"> 車用信号機の黄ランプを表すピクチャボックス </param>
        /// <param name="pib_red">    車用信号機の赤ランプを表すピクチャボックス </param>
        /// <param name="pib_arrow">  矢印信号機を表すピクチャボックス           </param>
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangeSignalLightOn(LightState state, PictureBox pib_green, PictureBox pib_yellow, PictureBox pib_red, PictureBox pib_arrow)
        {
            try
            {
                bool greVisible = false;  // 車用信号機の緑ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool yelVisible = false;  // 車用信号機の黄ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool redVisible = false;  // 車用信号機の赤ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool arwVisible = false;  // 矢印信号機のランプを点灯する場合はtrue、　それ以外の場合はfalse
                if (state == LightState.Green)  greVisible = true;
                if (state == LightState.Yellow) yelVisible = true;
                if (state == LightState.Red || state == LightState.Arrow) redVisible = true;
                if (state == LightState.Arrow)  arwVisible = true;

                pib_green.Visible  = greVisible;                        // 車用信号機緑ランプの点灯状態変更
                pib_yellow.Visible = yelVisible;                        // 車用信号機黄ランプの点灯状態変更
                pib_red.Visible    = redVisible;                        // 車用信号機赤ランプの点灯状態変更
                if (pib_arrow != null) pib_arrow.Visible = arwVisible;  // 矢印信号機の点灯状態変更
                return true; 
            }
            catch (Exception ex)
            {
                string exStr = ex.Message +"\n車用信号機点灯状態の更新でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 歩行者用信号機の点灯状態更新
        /// </summary>
        /// <param name="state">        点灯状態を表す列挙型                                   </param>
        /// <param name="pib_greenOne"> 歩行者用信号機の緑ランプを表す１つ目のピクチャボックス </param>
        /// <param name="pib_greenTwo"> 歩行者用信号機の緑ランプを表す２つ目のピクチャボックス </param>
        /// <param name="pib_redOne">   歩行者用信号機の赤ランプを表す１つ目のピクチャボックス </param>
        /// <param name="pib_redTwo">   歩行者用信号機の赤ランプを表す２つ目のピクチャボックス </param>
        /// <returns> 点灯状態更新に成功した場合はtrue、それ以外の場合はfalse </returns>
        private bool ChangePedesLightOn(LightState state, PictureBox pib_greenOne, PictureBox pib_greenTwo, PictureBox pib_redOne, PictureBox pib_redTwo)
        {
            try
            {
                bool greVisible = false;  // 歩行者用信号機の緑ランプを点灯する場合はtrue、それ以外の場合はfalse
                bool redVisible = false;  // 歩行者用信号機の赤ランプを点灯する場合はtrue、それ以外の場合はfalse
                if (state == LightState.Green) greVisible = true;
                if (state == LightState.Red)   redVisible = true;

                pib_greenOne.Visible = greVisible;  // １つ目歩行者用信号機の緑ランプの点灯状態変更
                pib_redOne.Visible   = redVisible;  // １つ目歩行者用信号機の赤ランプの点灯状態変更
                pib_greenTwo.Visible = greVisible;  // ２つ目歩行者用信号機の緑ランプの点灯状態変更
                pib_redTwo.Visible   = redVisible;  // ２つ目歩行者用信号機の赤ランプの点灯状態変更
                return true;
            }
            catch (Exception ex)
            {
                string exStr = ex.Message + "\n歩行者用信号機点灯状態の更新でエラーが発生しました。";
                MessageBox.Show(exStr, Program.SoftTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }
    }
}