
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The battle phase is handled by the DiscoveryController.
/// </summary>
static class DiscoveryController
{

	public static int hintsBtnLeft = 340;
	public static int hintsBtnTop = 70;
	public static int hintsBtnWidth = 64;
	public static int hintsBtnHeight = 64;
	public static int hintY = 0;
	public static int hintX = 0;
	public static bool hasHint = false;

	private const int BACK_BUTTON_X = 50;
	private const int BACK_BUTTON_Y = 20;
	private const int BACK_BUTTON_HEIGHT = 46;
	private const int BACK_BUTTON_WIDTH = 80;

	/// <summary>
	/// Handles input during the discovery phase of the game.
	/// </summary>
	/// <remarks>
	/// Escape opens the game menu. Clicking the mouse will
	/// attack a location.
	/// </remarks>
	public static void HandleDiscoveryInput()
	{
		if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE)) {
			GameController.AddNewState(GameState.ViewingGameMenu);
		}

		if (SwinGame.MouseClicked(MouseButton.LeftButton)) {
			if (UtilityFunctions.IsMouseInRectangle (hintsBtnLeft, hintsBtnTop, hintsBtnWidth, hintsBtnHeight)) {
				List<int> hints = GameController.ComputerPlayer.getHint ();
				Console.WriteLine (hints [0]);
				Console.WriteLine (hints [1]);

				hintY = UtilityFunctions.FIELD_LEFT + hints [1] * (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP);
				hintX = UtilityFunctions.FIELD_TOP + hints [0] * (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP);

				hasHint = true;
			} 
			else if (GameController.HumanPlayer.ReadyToDeploy & UtilityFunctions.IsMouseInRectangle (BACK_BUTTON_X, BACK_BUTTON_Y, BACK_BUTTON_WIDTH, BACK_BUTTON_HEIGHT)) {
				GameController.AddNewState (GameState.ViewingGameMenu);
			} 
			else {
				DoAttack ();
			}
		}

		Point2D mouse = SwinGame.MousePosition ();
		if (SwinGame.MouseClicked (MouseButton.LeftButton) && mouse.X > UtilityFunctions.FIELD_LEFT + 340 && mouse.Y > UtilityFunctions.FIELD_TOP - 50 && mouse.X < UtilityFunctions.FIELD_LEFT + 340 + 80 && mouse.Y < UtilityFunctions.FIELD_TOP - 50 + 46) {
			GameController.HumanPlayer.Reset ();
			GameController.ComputerPlayer.Reset ();
		}
		if (SwinGame.KeyDown (KeyCode.vk_r)) {
			GameController.HumanPlayer.Reset ();
			GameController.ComputerPlayer.Reset ();
		}
	}

	/// <summary>
	/// Attack the location that the mouse if over.
	/// </summary>
	private static void DoAttack()
	{
		Point2D mouse = default(Point2D);

		mouse = SwinGame.MousePosition();

		//Calculate the row/col clicked
		int row = 0;
		int col = 0;
		row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
		col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

		if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height) {
			if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width) {
				GameController.Attack(row, col);
			}
		}
	}

	/// <summary>
	/// Draws the game during the attack phase.
	/// </summary>s
	public static void DrawDiscovery()
	{
		const int SCORES_LEFT = 172;
		const int SHOTS_TOP = 157;
		const int HITS_TOP = 206;
		const int SPLASH_TOP = 256;

		SwinGame.DrawBitmap (GameResources.GameImage ("hints"), hintsBtnLeft, hintsBtnTop);
		if (hasHint) {
			SwinGame.DrawRectangle (Color.Yellow, hintY, hintX, UtilityFunctions.CELL_WIDTH, UtilityFunctions.CELL_HEIGHT);
		}

		if ((SwinGame.KeyDown(KeyCode.vk_LSHIFT) | SwinGame.KeyDown(KeyCode.vk_RSHIFT)) & SwinGame.KeyDown(KeyCode.vk_c)) {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
		} else {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);
		}

		SwinGame.DrawBitmap (GameResources.GameImage ("ResetButton"), UtilityFunctions.FIELD_LEFT + 340, UtilityFunctions.FIELD_TOP - 50);

		UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
		UtilityFunctions.DrawMessage();

		SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White,GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
