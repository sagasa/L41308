using System;
using System.Text;
using DxLibDLL;
using SAGASALib;
using KUMALib;
using Giraffe.Saves;

namespace Giraffe
{
    public class SceneResult : Scene
    {
        private ScenePlay _scenePlay;

        private const string HIGHTSCORE = "hightscore";
        HightScore.Entry entry = new HightScore.Entry();
        int scoreRank = 10;//自分の順位
        int timeRank = 10;
        private bool nameGet;//名前入力するか

        private enum Eval { a, b, c, none }//評価
        private Eval scoreEval;
        private Eval timeEval;
        private int[] evalScores = new int[] { 1100, 700, 400 };
        private DateTime[] evalTimes = new DateTime[] { new DateTime(1, 1, 1, 0, 1, 0), new DateTime(1, 1, 1, 0, 2, 0), new DateTime(1, 1, 1, 0, 3, 0) };
        private int[] timeBonus = new int[] { 1000, 500, 200 };//タイムボーナス用
        
        //カーソル
        //名前入力で使用
        private enum NameGet_XNum { Change, Decision }
        private NameGet_XNum nameGet_XNum;
        private readonly int[] nameGet_X = new int[] { 170, Screen.Width - 170 };
        private const int nameGet_Y = Screen.Height - 200;//位置調整中
        //ランキングで使用
        private const int ranking_X = Screen.Width / 2;
        private const int ranking_Y = Screen.Height - 50;
        //リザルトで使用
        private enum Result_XNum { Restart, Ranking, Back }
        private Result_XNum result_XNum;
        private readonly int[] result_X = new int[] { 100, Screen.Width / 2, Screen.Width - 100 };//位置調整中
        private const int result_Y = Screen.Height - 200;
        
        private int counter = 0;//wait,fade,blinkのカウンター
        private const int fadeTime = 120;
        //描画用定数
        private const int frameX = 240;
        private const int frameY = 100;
        private const int fontInterval1 = 30;//文字の間隔
        private const float fontScale1 = 0.18f;//文字の拡縮
        private const int fontInterval2 = fontInterval1 / 2;//タイムボーナス用
        private const float fontScale2 = 0.1f;
        private const float rankImageScale = 0.4f;
        private const float rankAnimationSpeed = 0.01f;

        private bool blinkMessage = true;//点滅表示用
        private float rankAnimationScale = 1;//アニメーション用
        private bool rankExpansionAnimation = false;
        private int playerAnimationTime = 100;
        private int playerMoveCounter = 0;
        private const int cursorWidth = 220;


        private bool playerMove = false;
        private bool playerOnRight = true;
        private DummyPlayer dummyPlayer;

        private const string numImage = "image_result/num_";

        private int shadow = ResourceLoader.GetGraph("image_result/shadow25.png");
        private int cursor = ResourceLoader.GetGraph("image_result/cursor.png");
        private int colon = ResourceLoader.GetGraph("image_result/rcolon.png");
        private int newImage = ResourceLoader.GetGraph("image_result/new.png");

        enum State
        {
            NameGet,
            Ranking,
            Result
        }
        State state;

        public SceneResult(Game game, ScenePlay scenePlay, int score, DateTime time) : base(game)
        {
            _scenePlay = scenePlay;
            dummyPlayer = new DummyPlayer(this);
            entry.name = Game.settings.nickname;
            entry.score = score;
            entry.timeBinary = time.ToBinary();
            entry.dateBinary = DateTime.Now.ToBinary();
        }

        public override void OnLoad()
        {
            DX.InitFontToHandle();
            DX.SetFontSize(50);//文字サイズの指定
            //文字とか枠とかの色の指定
            DX.SetKeyInputStringColor(DX.GetColor(63, 42, 11),/*入力文字列の色*/
                                      DX.GetColor(63, 42, 11),/*ＩＭＥ非使用時のカーソルの色*/
                                      DX.GetColor(255, 255, 255),/*ＩＭＥ使用時の入力文字列の周りの色*/
                                      DX.GetColor(63, 42, 11),/*ＩＭＥ使用時のカーソルの色*/
                                      DX.GetColor(63, 42, 11),/*ＩＭＥ使用時の変換文字列の下線*/
                                      DX.GetColor(255, 255, 255),/*ＩＭＥ使用時の選択対象の変換候補文字列の色*/
                                      DX.GetColor(63, 42, 11),/*ＩＭＥ使用時の入力モード文字列の色(『全角ひらがな』等)*/
                                      DX.GetColor(255, 255, 255),/*入力文字列の縁の色*/
                                      DX.GetColor(255, 255, 255),/*ＩＭＥ使用時の選択対象の変換候補文字列の縁の色*/
                                      DX.GetColor(255, 255, 255),/*ＩＭＥ使用時の入力モード文字列の縁の色*/
                                      DX.GetColor(63, 42, 11),/*ＩＭＥ使用時の変換ウインドウの縁の色*/
                                      DX.GetColor(255, 246, 170),/*ＩＭＥ使用時の変換ウインドウの下地の色*/
                                      DX.GetColor(255, 255, 255),/*入力文字列の選択部分(SHIFTキーを押しながら左右キーで選択)の周りの色*/
                                      DX.GetColor(255, 255, 255),/*入力文字列の選択部分(SHIFTキーを押しながら左右キーで選択)の色*/
                                      DX.GetColor(255, 255, 255));/*入力文字列の選択部分(SHIFTキーを押しながら左右キーで選択)の縁の色*/

            for (int i = 0; i < evalTimes.Length; i++)//タイムの評価
            {
                if (evalTimes[i] >= DateTime.FromBinary(entry.timeBinary))
                {
                    timeEval = (Eval)i;
                    entry.score += timeBonus[i];
                    break;
                }
                if (i == evalTimes.Length - 1)
                    timeEval = Eval.none;
            }
            for (int i = 0; i < evalScores.Length; i++)//スコアの評価
            {
                if (evalScores[i] <= entry.score)
                {
                    scoreEval = (Eval)i;
                    break;
                }
                if (i == evalScores.Length - 1)
                    scoreEval = Eval.none;
            }

            if (Game.hightScore.BreakRecord(entry, _scenePlay.StageNum))//記録を更新してるか
            {
                state = State.NameGet;
                if (Game.hightScore.BreakRecord(entry, _scenePlay.StageNum))
                {
                    state = State.NameGet;
                    if (entry.name == null)
                        nameGet = true;
                    else
                    {
                        nameGet = false;
                        nameGet_XNum = NameGet_XNum.Change;
                    }
                }
            }
            else
            {
                state = State.Ranking;
            }
            
            

            //描画系の初期化　しなくてよいかも
            blinkMessage = true;
            counter = 0;
            rankAnimationScale = 1;
            rankExpansionAnimation = false;

            //カーソルとダミープレイヤーの位置の初期化
            //dummyPlayer.pos = new Vec2f(cursorPosX, result_Y - 85);
        }

        public override void Update()
        {
            counter++;
            //newの描画に使用
            if (counter % 60 == 0)
                blinkMessage = true;
            else if (counter % 60 == 40)
                blinkMessage = false;
            //ランクの描画に使用
            if (rankExpansionAnimation)
                rankAnimationScale += rankAnimationSpeed;
            else if (!rankExpansionAnimation)
                rankAnimationScale -= rankAnimationSpeed;
            if (rankAnimationScale > 1)
                rankExpansionAnimation = false;
            else if (rankAnimationScale < 0.75)
                rankExpansionAnimation = true;

            if (!Game.fadeAction)
            {
                if (state == State.NameGet)
                {
                    if (nameGet)
                    {
                        DX.SetFontSize(50);//文字サイズの指定
                        //文字とか枠とかの色の指定
                        DX.SetKeyInputStringColor(DX.GetColor(63, 42, 11),/*入力文字列の色*/
                                                  DX.GetColor(63, 42, 11),/*ＩＭＥ非使用時のカーソルの色*/
                                                  DX.GetColor(255, 255, 255),/*ＩＭＥ使用時の入力文字列の周りの色*/
                                                  DX.GetColor(63, 42, 11),/*ＩＭＥ使用時のカーソルの色*/
                                                  DX.GetColor(63, 42, 11),/*ＩＭＥ使用時の変換文字列の下線*/
                                                  DX.GetColor(255, 255, 255),/*ＩＭＥ使用時の選択対象の変換候補文字列の色*/
                                                  DX.GetColor(63, 42, 11),/*ＩＭＥ使用時の入力モード文字列の色(『全角ひらがな』等)*/
                                                  DX.GetColor(255, 255, 255),/*入力文字列の縁の色*/
                                                  DX.GetColor(255, 255, 255),/*ＩＭＥ使用時の選択対象の変換候補文字列の縁の色*/
                                                  DX.GetColor(255, 255, 255),/*ＩＭＥ使用時の入力モード文字列の縁の色*/
                                                  DX.GetColor(63, 42, 11),/*ＩＭＥ使用時の変換ウインドウの縁の色*/
                                                  DX.GetColor(255, 246, 170),/*ＩＭＥ使用時の変換ウインドウの下地の色*/
                                                  DX.GetColor(255, 255, 255),/*入力文字列の選択部分(SHIFTキーを押しながら左右キーで選択)の周りの色*/
                                                  DX.GetColor(255, 255, 255),/*入力文字列の選択部分(SHIFTキーを押しながら左右キーで選択)の色*/
                                                  DX.GetColor(255, 255, 255));/*入力文字列の選択部分(SHIFTキーを押しながら左右キーで選択)の縁の色*/
                        StringBuilder nickname_ = new StringBuilder();
                        //X座標,Y座標,入力可能文字数,保存する場所,ESCでキャンセルできる(ようにする)か
                        if (DX.KeyInputString(110, Screen.Height / 2 - 40, 8, nickname_, DX.TRUE) == DX.TRUE)
                            entry.name = nickname_.ToString();
                        if (entry.name == null)
                            nameGet_XNum = NameGet_XNum.Change;
                        else
                            nameGet_XNum = NameGet_XNum.Decision;
                        nameGet = false;
                    }
                    else if (nameGet_XNum != NameGet_XNum.Decision && Input.RIGHT.IsPush())//カーソルの移動
                    {
                        Sound.Play("cursor_SE.mp3");
                        nameGet_XNum++;
                    }
                    else if (nameGet_XNum != NameGet_XNum.Change && Input.LEFT.IsPush())
                    {
                        Sound.Play("cursor_SE.mp3");
                        nameGet_XNum--;
                    }
                    else if (nameGet_XNum == NameGet_XNum.Change && Input.ACTION.IsPush())//カーソルが変更のとき
                    {
                        Sound.Play("decision_SE.mp3");
                        nameGet = true;
                    }
                    else if (nameGet_XNum == NameGet_XNum.Decision && Input.ACTION.IsPush())//カーソルが決定のとき
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.settings.nickname = entry.name;
                        Game.hightScore.RankingSort(entry, _scenePlay.StageNum, ref scoreRank, ref timeRank);
                        state = State.Ranking;
                    }
                }
                else if (state == State.Ranking)
                {
                    //画面スクロール

                    //ランキングが表示しきれなかったら横スクロール出来るようにする

                    if (Input.ACTION.IsPush())//画面スクロールが終わっているときにスペースキー
                    {
                        state = State.Result;
                        result_XNum = Result_XNum.Ranking;
                        //ダミープレイヤーの位置の初期化
                    }
                }
                else if (state == State.Result)
                {
                    if (result_XNum != Result_XNum.Back && Input.RIGHT.IsPush())
                    {
                        Sound.Play("cursor_SE.mp3");
                        result_XNum++;
                    }
                    else if (result_XNum != Result_XNum.Restart && Input.LEFT.IsPush())
                    {
                        Sound.Play("cursor_SE.mp3");
                        result_XNum--;
                    }
                    else if (result_XNum == Result_XNum.Restart && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.fadeAction = true;
                        Game.bgmManager.Set(fadeTime, "play", "result");
                        Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.CrossFade);
                        Game.SetScene(new ScenePlay(Game, _scenePlay.Map, _scenePlay.ResourcesName, _scenePlay.StageNum), new Fade(fadeTime, true, true));
                    }
                    else if (result_XNum == Result_XNum.Ranking && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        state = State.Ranking;
                    }
                    else if (result_XNum == Result_XNum.Back && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.fadeAction = true;
                        Game.bgmManager.Set(fadeTime, "title", "result");
                        Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.CrossFade);
                        Game.SetScene(new Title(Game, 1, "_0"), new Fade(fadeTime, true, true));
                    }
                }
            }


            /*
            if (!playerMove)
            {
                if (playerOnRight && cursorPosX == fixedPosX[0])
                {
                    playerMove = true;
                }
                else if (!playerOnRight && cursorPosX == fixedPosX[1])
                {
                    playerMove = true;
                }
            }
            else if (playerMove)
            {
                if (playerOnRight && cursorPosX == fixedPosX[1])
                {
                    playerMove = false;
                }
                else if (!playerOnRight && cursorPosX == fixedPosX[0])
                {
                    playerMove = false;
                }
            }

            if (playerMove)
            {
                if (playerOnRight)
                {
                    dummyPlayer.AnimationManager.Start(Animations.Test2);
                    dummyPlayer.isDunnyRight = false;
                    playerMoveCounter++;
                    if (playerMoveCounter == playerAnimationTime)
                    {
                        playerOnRight = !playerOnRight;
                        playerMoveCounter = 0;
                    }
                }
                else
                {
                    dummyPlayer.isDunnyRight = true;
                    dummyPlayer.AnimationManager.Start(Animations.Test);
                    playerMoveCounter++;
                    if (playerMoveCounter == playerAnimationTime)
                    {
                        playerOnRight = !playerOnRight;
                        playerMoveCounter = 0;
                    }
                }
                //if (dummyPlayer.pos.X < fixedPosX[0])
                //{
                //    dummyPlayer.pos.X
                //}
            }

            else if (!Game.fadeAction)
            {
                if (Input.LEFT.IsPush())
                {
                    dummyPlayer.isDunnyRight = false;
                }
                else if (Input.RIGHT.IsPush())
                {
                    dummyPlayer.isDunnyRight = true;
                }
            }

            dummyPlayer.Update();
            
            if (cursorPosX == result_X[result_X.Length - 1] && (Input.RIGHT.IsPush() || Input.RIGHT.IsHold()))
            {
                if (playerMove)
                {
                    dummyPlayer.isDunnyRight = true;
                }
            }
            */

        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, ResourceLoader.GetGraph("tree_top_" + _scenePlay.StageNum + ".png"));
            DX.DrawGraph(0, 0, ResourceLoader.GetGraph("image_result/result_bg.png"));
            
            int scoreLeftCounter = 0;
            int bonusLeftCounter = 0;
            int timeLeftCounter = 0;
            NumberDraw.ScoreDraw(entry.score, frameX, frameY, fontInterval1, fontScale1, numImage, ref scoreLeftCounter);
            NumberDraw.TimeDraw(DateTime.FromBinary(entry.timeBinary), frameX, frameY + 200, fontInterval1, fontScale1, numImage, ref timeLeftCounter);
            
            if (scoreEval != Eval.none)//スコアの評価
            {
                DX.DrawRotaGraph(70, 95, rankImageScale * rankAnimationScale, 0, ResourceLoader.GetGraph("image_result/rank_" + (int)scoreEval + ".png"));
            }
            if (timeEval != Eval.none)//タイムの評価
            {
                DX.DrawRotaGraph(70, 190, rankImageScale * rankAnimationScale, 0, ResourceLoader.GetGraph("image_result/rank_" + (int)timeEval + ".png"));
                NumberDraw.ScoreDraw(timeBonus[(int)timeEval], frameX + fontInterval1 * scoreLeftCounter, frameY + 40, fontInterval2, fontScale2, numImage, ref bonusLeftCounter);
                //()と+
                DX.DrawRotaGraph(frameX + fontInterval1 * scoreLeftCounter - fontInterval1 / 3, frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/brackets(.png"));
                DX.DrawRotaGraph(frameX + fontInterval1 * scoreLeftCounter, frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/plus.png"));
                DX.DrawRotaGraph(frameX + fontInterval1 * scoreLeftCounter + fontInterval2 * (bonusLeftCounter + 1), frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/brackets).png"));
            }

            if (state == State.NameGet)
            {
                DX.DrawGraph(0, 0, shadow);
                if (!nameGet)
                {
                    DX.DrawRotaGraph(nameGet_X[(int)nameGet_XNum], nameGet_Y, 1, 0, cursor);
                }
                else
                {
                    //ネームスペース用のカーソル
                }
                DX.DrawGraph(0, 0, ResourceLoader.GetGraph("image_result/nameget_message.png"));
                DX.DrawRotaGraph(Screen.Width / 2, Screen.Height / 2, 1, 0, ResourceLoader.GetGraph("image_result/name_space.png"));
                if (!nameGet)//入力した名前を表示
                    DX.DrawString(110, Screen.Height / 2 - 40, entry.name, DX.GetColor(63, 42, 11));
                DX.DrawRotaGraph(nameGet_X[0], nameGet_Y, 1, 0, ResourceLoader.GetGraph("image_result/change.png"));
                DX.DrawRotaGraph(nameGet_X[1], nameGet_Y, 1, 0, ResourceLoader.GetGraph("image_result/decision.png"));
            }
            else if (state == State.Ranking)
            {
                DX.DrawGraph(0, 0, shadow);
                DX.DrawRotaGraph(ranking_X, ranking_Y, 1, 0, cursor);
                DX.DrawGraph(0, -50, ResourceLoader.GetGraph("image_result/ranking_bg.png"));
                DX.DrawRotaGraph(ranking_X, ranking_Y, 1, 0, ResourceLoader.GetGraph("image_result/back.png"));

                //ランキングの表示
                Game.hightScore.ScoreRankingDraw(_scenePlay.StageNum, scoreRank);
            }
            else if (state == State.Result)
            {
                dummyPlayer.Draw();
                DX.DrawRotaGraph(result_X[(int)result_XNum], result_Y, 1, 0, cursor);
                DX.DrawRotaGraph(result_X[0], result_Y, 1, 0, ResourceLoader.GetGraph("image_result/restart.png"));
                DX.DrawRotaGraph(result_X[1], result_Y, 1, 0, ResourceLoader.GetGraph("image_result/ranking.png"));
                DX.DrawRotaGraph(result_X[2], result_Y, 1, 0, ResourceLoader.GetGraph("image_result/back.png"));
            }
        }

        public override void OnExit()
        {
            Game.fadeAction = false;
        }
    }
}