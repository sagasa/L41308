using System;
using System.Text;
using DxLibDLL;
using SAGASALib;
using Giraffe.Saves;

namespace Giraffe
{
    public class SceneResult : Scene
    {
        private const string HIGHTSCORE = "hightscore";

        private ScenePlay _scenePlay;
        int currentScore = 0;
        int bestScore = 0;
        int[] currentTime = new int[] { 0, 0, 0 };
        int[] bestTime = new int[] { 0, 0, 0 };

        private string[] ranks = new string[] { "a", "b", "c" };
        private int[] rankScore = new int[] { 1100, 700, 400 };//スコア評価用,数値は仮
        private int[] rankTime = new int[] { 60, 120, 180 };//タイム評価用
        private int[] timeBonus = new int[] { 1000, 500, 200 };//タイムボーナス用
        private string timeRank = "d";
        private string scoreRank = "d";

        private int Counter = 0;//wait,fade,blinkのカウンター
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
        private int cursorPosX;
        private const int cursorPosY = Screen.Height - 200;
        private readonly int[] fixedPosX = new int[] { 170, Screen.Width - 170 };
        private bool playerMove = false;
        private bool playerOnRight = true;
        private DummyPlayer dummyPlayer;

        private bool nameGet = false;
        StringBuilder nickname = new StringBuilder("");

        private int cursor = ResourceLoader.GetGraph("image_result/cursor.png");
        private int coron = ResourceLoader.GetGraph("image_result/rcolon.png");
        private int newImage = ResourceLoader.GetGraph("image_result/new.png");
        

        public SceneResult(Game game, ScenePlay scenePlay, DateTime minValue) : base(game)
        {
            _scenePlay = scenePlay;
            dummyPlayer = new DummyPlayer(this);
        }

        public override void OnLoad()
        {
            blinkMessage = true;
            Counter = 0;
            rankAnimationScale = 1;
            rankExpansionAnimation = false;

            cursorPosX = fixedPosX[1];
            dummyPlayer.pos = new Vec2f(cursorPosX, cursorPosY - 85);

            currentScore = Game.currentScore;
            currentTime = Game.currentTime;
            bestScore = Game.hightScore.bestScores["stage" + _scenePlay.ResourcesName];
            bestTime = Game.hightScore.bestTimes["stage" + _scenePlay.ResourcesName];

            for (int i = 0; i < rankTime.Length; i++)//タイムの評価
            {
                if (rankTime[i] >= currentTime[0] * 60 + currentTime[1])
                {
                    currentScore += timeBonus[i];
                    timeRank = ranks[i];
                    break;
                }
            }
            for (int i = 0; i < rankScore.Length; i++)//スコアの評価
            {
                if (rankScore[i] <= currentScore)
                {
                    scoreRank = ranks[i];
                    break;
                }
            }

            nameGet = false;
            if (currentScore > bestScore)
            {
                Game.hightScore.bestScores["stage" + _scenePlay.ResourcesName] = currentScore;
                nameGet = true;
            }
            if (currentTime[0] * 60 + currentTime[1] < bestTime[0] * 60 + bestTime[1])
            {
                Game.hightScore.bestTimes["stage" + _scenePlay.ResourcesName] = currentTime;
                nameGet = true;
            }
            #if !DEBUG
            SaveManager.Save(HIGHTSCORE, Game.hightScore);
            #endif
        }

        public override void Update()
        {
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

            Counter++;
            if (Counter < fadeTime + 10)
                Game.bgmManager.FadeIn("result", 120);

            if (Counter % 60 == 0)
                blinkMessage = true;
            else if (Counter % 60 == 40)
                blinkMessage = false;
            //ランクのアニメーションに使用
            if (rankExpansionAnimation)
                rankAnimationScale += rankAnimationSpeed;
            else if (!rankExpansionAnimation)
                rankAnimationScale -= rankAnimationSpeed;
            if (rankAnimationScale > 1)
                rankExpansionAnimation = false;
            else if (rankAnimationScale < 0.75)
                rankExpansionAnimation = true;
            //名前入力
            if (!Game.fadeAction && nameGet)
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
                nameGet = false;
            }

            else if (!Game.fadeAction)
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
                    Game.bgmManager.currentScene = "result";
                    Game.SetScene(new ScenePlay(Game, _scenePlay.Map, _scenePlay.ResourcesName), new Fade(fadeTime, true, true));
                }
                else if (cursorPosX == fixedPosX[1] && Input.ACTION.IsPush())
                {
                    Sound.Play("decision_SE.mp3");
                    Game.fadeAction = true;
                    Game.bgmManager.currentScene = "result";
                    Game.SetScene(new Title(Game), new Fade(fadeTime, true, true));
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
            //スコア
            NumberDraw.ScoreDraw(bestScore, frameX, frameY + 50, fontInterval1, fontScale1, "image_result/result_num_");
            NumberDraw.ScoreDraw(currentScore, frameX, frameY, fontInterval1, fontScale1, "image_result/result_num_", ref scoreLeftCounter);
            //タイム
            NumberDraw.TimeDraw(bestTime, frameX, 246, fontInterval1, fontScale1, "image_result/result_num_", ref timeLeftCounter);
            DX.DrawRotaGraph(frameX + fontInterval1 * timeLeftCounter, 246, 0.2, 0, coron);
            NumberDraw.TimeDraw(currentTime, frameX, 200, fontInterval1, fontScale1, "image_result/result_num_", ref timeLeftCounter);
            DX.DrawRotaGraph(frameX + fontInterval1 * timeLeftCounter, 200, 0.2, 0, coron);
            //評価とタイムボーナス
            for (int i = 0; i < ranks.Length; i++)
            {
                if (timeRank == ranks[i])
                {   //タイムボーナス
                    NumberDraw.ScoreDraw(timeBonus[i], frameX + fontInterval2 + fontInterval1 * scoreLeftCounter, frameY + 5,
                        fontInterval2, fontScale2, "image_result/result_num_", ref bonusLeftCounter);
                    // ()と+
                    DX.DrawRotaGraph(frameX + fontInterval1 * scoreLeftCounter - fontInterval1 / 3, frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/brackets1.png"));
                    DX.DrawRotaGraph(frameX + fontInterval1 * scoreLeftCounter, frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/plus.png"));
                    DX.DrawRotaGraph(frameX + fontInterval1 * scoreLeftCounter + fontInterval2 * (bonusLeftCounter + 1), frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/brackets2.png"));
                    //タイム評価
                    DX.DrawRotaGraph(70, 190, rankImageScale * rankAnimationScale, 0, ResourceLoader.GetGraph("image_result/rank_" + ranks[i] + ".png"));
                }
                if (scoreRank == ranks[i])//スコア評価
                    DX.DrawRotaGraph(70, 95, rankImageScale * rankAnimationScale, 0, ResourceLoader.GetGraph("image_result/rank_" + ranks[i] + ".png"));
            }
            if (blinkMessage && currentScore > bestScore)
            {//スコアの「new」
                DX.DrawRotaGraph(frameX + 65 + fontInterval1 * (scoreLeftCounter + 1) + fontInterval1 / 2 * bonusLeftCounter, frameY, 1, 0, newImage);
            }
            if (blinkMessage && (currentTime[0] * 3600 + currentTime[1] * 60 + currentTime[2] < bestTime[0] * 3600 + bestTime[1] * 60 + bestTime[2]))
            {//タイムの「new」
                DX.DrawRotaGraph(frameX + 65 + fontInterval1 * (timeLeftCounter + 3), 200, 1, 0, newImage);
            }

            if (!nameGet)
            {
                dummyPlayer.Draw();
                DX.DrawRotaGraph(cursorPosX, cursorPosY, 1, 0, cursor);
                DX.DrawRotaGraph(fixedPosX[0], cursorPosY, 1, 0, ResourceLoader.GetGraph("image_result/restart.png"));
                DX.DrawRotaGraph(fixedPosX[1], cursorPosY, 1, 0, ResourceLoader.GetGraph("image_result/r_back.png"));
            }

            if (!Game.fadeAction && nameGet)
            {
                DX.DrawGraph(0, 0, ResourceLoader.GetGraph("image_result/shadow25.png"));
                DX.DrawRotaGraph(Screen.Width / 2, Screen.Height / 2, 1, 0, ResourceLoader.GetGraph("image_result/name_space.png"));
            }
        }

        public override void OnExit()
        {
            Game.fadeAction = false;
        }
    }
}