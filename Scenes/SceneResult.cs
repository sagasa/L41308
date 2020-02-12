using System;
using System.Text;
using DxLibDLL;
using SAGASALib;
using Giraffe.Saves;

namespace Giraffe
{
    public class SceneResult : Scene
    {
        private ScenePlay _scenePlay;
        private const string HIGHTSCORE = "hightscore";
        
        private int[] evalScores = new int[] { 1100, 700, 400 };//評価用
        private DateTime[] evalTimes = new DateTime[] { new DateTime(0, 0, 0, 0, 1, 0), new DateTime(0, 0, 0, 0, 2, 0), new DateTime(0, 0, 0, 0, 3, 0) };
        private int[] timeBonus = new int[] { 1000, 500, 200 };//タイムボーナス用

        HightScore.Entry entry;
        private int scoreEval;//自分の評価
        private int timeEval;
        int scoreRank = 10;//自分の順位
        int timeRank = 10;

        private bool nameGet;//名前入力するか
        StringBuilder nickname = new StringBuilder(null);//名前受け取り用

        //カーソル
        private int cursorPosX;
        private int[] nameGetCursorFixdPos = new int[] { 170, Screen.Width - 170 };

        private const int nameGetCursorPosY = Screen.Height - 200;//位置調整中
        private const int rankingCursorPosY = Screen.Height - 200;//位置調整中
        private const int resultCursorPosY = Screen.Height - 200;
        
        
       
        

        



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
        
        private readonly int[] fixedPosX = new int[] { 170, Screen.Width - 170 };
        private bool playerMove = false;
        private bool playerOnRight = true;
        private DummyPlayer dummyPlayer;
        
        private int cursor = ResourceLoader.GetGraph("image_result/cursor.png");
        private int coron = ResourceLoader.GetGraph("image_result/rcolon.png");
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
            entry.score = score;
            entry.timeBinary = time.ToBinary();
            entry.dateBinary = DateTime.Now.ToBinary();
        }

        public override void OnLoad()
        {
            if(Game.hightScore.BreakRecord(entry, _scenePlay.StageNum))//記録を更新してるか
            {
                state = State.NameGet;
                if (nickname == null)
                    nameGet = true;
                else
                {
                    nameGet = false;
                    cursorPosX = nameGetCursorFixdPos[0];
                }
            }
            else
            {
                state = State.Ranking;
            }

            for (int i = 0; i < evalScores.Length; i++)//スコアの評価
            {
                if (evalScores[i] <= entry.score)
                {
                    scoreEval = i;
                    break;
                }
                if (i == evalScores.Length - 1)
                    scoreEval = 100;
            }
            for (int i = 0; i < evalTimes.Length; i++)//タイムの評価
            {
                if (evalTimes[i] >= DateTime.FromBinary(entry.timeBinary))
                {
                    timeEval = i;
                    break;
                }
                if (i == evalTimes.Length - 1)
                    timeEval = 100;
            }

            //描画系の初期化　しなくてよいかも
            blinkMessage = true;
            counter = 0;
            rankAnimationScale = 1;
            rankExpansionAnimation = false;

            //カーソルとダミープレイヤーの位置の初期化
            dummyPlayer.pos = new Vec2f(cursorPosX, resultCursorPosY - 85);
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
                    //名前入力
                    if (nameGet)
                    {
                        //X座標,Y座標,入力可能文字数(全角は2文字扱い),保存する場所,ESCでキャンセルできる(ようにする)か
                        DX.SetFontSize(50);//文字サイズの指定
                                           //文字の色を指定
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
                        DX.KeyInputString(110, Screen.Height / 2 - 40, 8, nickname, DX.TRUE);
                        
                        cursorPosX = nameGetCursorFixdPos[1];//カーソルを決定に合わせる

                        nameGet = false;
                    }
                    else if (cursorPosX != nameGetCursorFixdPos[0] && Input.RIGHT.IsPush())//カーソルの移動
                    {
                        cursorPosX = nameGetCursorFixdPos[0];
                    }
                    else if (cursorPosX != nameGetCursorFixdPos[1] && Input.LEFT.IsPush())//カーソルの移動
                    {
                        cursorPosX = nameGetCursorFixdPos[1];
                    }
                    else if (cursorPosX == nameGetCursorFixdPos[0] && Input.ACTION.IsPush())//カーソルが変更のとき
                    {
                        nameGet = true;
                    }
                    else if (cursorPosX == nameGetCursorFixdPos[1] && Input.ACTION.IsPush())//カーソルが決定のとき
                    {
                        entry.name = nickname.ToString();
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
                    }
                }
                else if (state == State.Result)
                {

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
            */

            

            
            if (!Game.fadeAction)
            {

                if (cursorPosX != fixedPosX[0] && Input.LEFT.IsPush())//カーソルが一番左以外の時に←が押されたら、カーソルを一つ左へ
                {
                    Sound.Play("cursor_SE.mp3");
                    for (int i = 0; i < fixedPosX.Length; i++)
                    {
                        if (cursorPosX == fixedPosX[i])
                        {
                            cursorPosX = fixedPosX[i - 1];
                            break;
                        }
                    }
                }
                if (cursorPosX == fixedPosX[fixedPosX.Length - 1] && (Input.RIGHT.IsPush() || Input.RIGHT.IsHold()))
                {
                    if (playerMove)
                    {
                        dummyPlayer.isDunnyRight = true;
                    }
                }
                else if (cursorPosX != fixedPosX[fixedPosX.Length - 1] && Input.RIGHT.IsPush())//カーソルが一番右以外の時→を押されたら、カーソルを一つ右へ
                {
                    Sound.Play("cursor_SE.mp3");
                    for (int i = 0; i < fixedPosX.Length; i++)
                    {
                        if (cursorPosX == fixedPosX[i])
                        {
                            cursorPosX = fixedPosX[i + 1];
                            break;
                        }
                    }
                }
                if (cursorPosX == fixedPosX[0] && Input.ACTION.IsPush())
                {
                    Sound.Play("decision_SE.mp3");
                    Game.fadeAction = true;
                    Game.bgmManager.Set(fadeTime, "play", "result");
                    Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.CrossFade);
                    Game.SetScene(new ScenePlay(Game, _scenePlay.Map, _scenePlay.ResourcesName, _scenePlay.StageNum), new Fade(fadeTime, true, true));
                }
                else if (cursorPosX == fixedPosX[1] && Input.ACTION.IsPush())
                {
                    Sound.Play("decision_SE.mp3");
                    Game.fadeAction = true;
                    Game.bgmManager.Set(fadeTime, "title", "result");
                    Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.CrossFade);
                    Game.SetScene(new Title(Game, 1), new Fade(fadeTime, true, true));
                }
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, ResourceLoader.GetGraph("tree_top" + _scenePlay.ResourcesName + ".png"));
            DX.DrawGraph(0, 0, ResourceLoader.GetGraph("image_result/result_bg.png"));

            int scoreLeftCounter = 0;
            int bonusLeftCounter = 0;
            int timeLeftCounter = 0;
            ////スコア
            //NumberDraw.ScoreDraw(bestScore, frameX, frameY + 50, fontInterval1, fontScale1, "image_result/result_num_");
            //NumberDraw.ScoreDraw(currentScore, frameX, frameY, fontInterval1, fontScale1, "image_result/result_num_", ref scoreLeftCounter);
            ////タイム
            //NumberDraw.TimeDraw(bestTime, frameX, 246, fontInterval1, fontScale1, "image_result/result_num_", ref timeLeftCounter);
            //DX.DrawRotaGraph(frameX + fontInterval1 * timeLeftCounter, 246, 0.2, 0, coron);
            //NumberDraw.TimeDraw(currentTime, frameX, 200, fontInterval1, fontScale1, "image_result/result_num_", ref timeLeftCounter);
            //DX.DrawRotaGraph(frameX + fontInterval1 * timeLeftCounter, 200, 0.2, 0, coron);
            ////評価とタイムボーナス
            //for (int i = 0; i < ranks.Length; i++)
            //{
            //    if (timeEval == ranks[i])
            //    {   //タイムボーナス
            //        NumberDraw.ScoreDraw(timeBonus[i], frameX + fontInterval2 + fontInterval1 * scoreLeftCounter, frameY + 5,
            //            fontInterval2, fontScale2, "image_result/result_num_", ref bonusLeftCounter);
            //        // ()と+
            //        DX.DrawRotaGraph(frameX + fontInterval1 * scoreLeftCounter - fontInterval1 / 3, frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/brackets1.png"));
            //        DX.DrawRotaGraph(frameX + fontInterval1 * scoreLeftCounter, frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/plus.png"));
            //        DX.DrawRotaGraph(frameX + fontInterval1 * scoreLeftCounter + fontInterval2 * (bonusLeftCounter + 1), frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/brackets2.png"));
            //        //タイム評価
            //        DX.DrawRotaGraph(70, 190, rankImageScale * rankAnimationScale, 0, ResourceLoader.GetGraph("image_result/rank_" + ranks[i] + ".png"));
            //    }
            //}
            ////スコアの評価
            //DX.DrawRotaGraph(70, 95, rankImageScale * rankAnimationScale, 0, ResourceLoader.GetGraph("image_result/rank_" + scoreEval + ".png"));
            //if (blinkMessage && currentScore > bestScore)
            //{//スコアの「new」
            //    DX.DrawRotaGraph(frameX + 65 + fontInterval1 * (scoreLeftCounter + 1) + fontInterval1 / 2 * bonusLeftCounter, frameY, 1, 0, newImage);
            //}
            //if (blinkMessage && (currentTime[0] * 3600 + currentTime[1] * 60 + currentTime[2] < bestTime[0] * 3600 + bestTime[1] * 60 + bestTime[2]))
            //{//タイムの「new」
            //    DX.DrawRotaGraph(frameX + 65 + fontInterval1 * (timeLeftCounter + 3), 200, 1, 0, newImage);
            //}

            if (!nameGet)
            {

                dummyPlayer.Draw();
                DX.DrawRotaGraph(cursorPosX, resultCursorPosY, 1, 0, cursor);
                DX.DrawRotaGraph(fixedPosX[0], resultCursorPosY, 1, 0, ResourceLoader.GetGraph("image_result/restart.png"));
                DX.DrawRotaGraph(fixedPosX[1], resultCursorPosY, 1, 0, ResourceLoader.GetGraph("image_result/r_back.png"));
            }

            if (!Game.fadeAction && nameGet)
            {
                DX.DrawGraph(0, 0, ResourceLoader.GetGraph("image_result/shadow25.png"));
                DX.DrawGraph(0, 0, ResourceLoader.GetGraph("image_result/nameget_message.png"));
                //DX.DrawRotaGraph();
                //DX.DrawRotaGraph();
                DX.DrawRotaGraph(Screen.Width / 2, Screen.Height / 2, 1, 0, ResourceLoader.GetGraph("image_result/name_space.png"));
            }
        }

        public override void OnExit()
        {
            Game.fadeAction = false;
        }
    }
}