using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteForceCheck
{
    // Fungsi brute force untuk game
    public static bool IsMatch(string playerInput, string targetWord)
    {
        // Konversi ke char array biar sesuai flowchart
        char[] T = playerInput.ToCharArray();
        char[] P = targetWord.ToCharArray();

        int m = P.Length; // panjang kata
        int n = T.Length; // panjang input

        // Kalau panjang beda langsung false
        if (n != m) return false;

        // Loop pengecekan karakter demi karakter
        for (int j = 0; j < m; j++)
        {
            if (T[j] != P[j])
            {
                return false; // mismatch
            }
        }

        return true; // semua karakter cocok
    }

    // Contoh testing
    public static void Main(string[] args)
    {
        string meteorWord = "galaxy";
        string playerTyped = "galaxy";

        if (IsMatch(playerTyped, meteorWord))
        {
            Console.WriteLine("Match! Meteor hancur 🚀💥");
        }
        else
        {
            Console.WriteLine("Mismatch! Meteor masih meluncur 🌠");
        }
    }
}
