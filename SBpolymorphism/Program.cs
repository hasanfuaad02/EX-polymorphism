using System;
using System.Collections.Generic;

public abstract class Bank
{
    public abstract void SendTransfer(double amount, Bank recipient);
    public abstract void ReceiveTransfer(double amount, Bank sender);
    public abstract double GetBalance();
}

public class Account : Bank
{
    private double balance;

    public Account(double initialBalance) => Balance = initialBalance;

    private double Balance
    {
        get => balance;
        set => balance = Math.Max(value, 0); // Ensure balance is non-negative
    }

    public override void SendTransfer(double amount, Bank recipient)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            recipient.ReceiveTransfer(amount, this);
            Console.WriteLine($"Transfer of ${amount} sent.");
        }
        else
        {
            Console.WriteLine("Insufficient funds for the transfer.");
        }
    }

    public override void ReceiveTransfer(double amount, Bank sender)
    {
        Balance += amount;
        Console.WriteLine($"Received ${amount} from {sender}.");
    }

    public override double GetBalance() => Balance;

    public override string ToString() => $"Account with balance ${Balance}";
}

public class SmartAccount : Account
{
    private int rewardsPoints;

    public SmartAccount(double initialBalance, int initialRewardsPoints) : base(initialBalance) =>
        RewardsPoints = initialRewardsPoints;

    private int RewardsPoints
    {
        get => rewardsPoints;
        set => rewardsPoints = Math.Max(value, 0); // Ensure rewardsPoints is non-negative
    }

    public override void ReceiveTransfer(double amount, Bank sender)
    {
        base.ReceiveTransfer(amount, sender);
        RewardsPoints += (int)(amount * 0.1);
        Console.WriteLine($"Earned {amount * 0.1} rewards points. Total points: {RewardsPoints}");
    }

    public override string ToString() => $"SmartAccount with balance ${GetBalance()} and {RewardsPoints} rewards points";
}

class Program
{
    static void Main()
    {
        List<Bank> accounts = new List<Bank>
        {
            new Account(1000),
            new SmartAccount(500, 50)
        };

        foreach (var account in accounts)
        {
            Console.WriteLine(account);
        }

        accounts[0].SendTransfer(200, accounts[1]);
        Console.WriteLine(accounts[0].GetBalance());
        Console.WriteLine(accounts[1].GetBalance());

        accounts[1].SendTransfer(50, accounts[0]);
        Console.WriteLine(accounts[0].GetBalance());
        Console.WriteLine(accounts[1].GetBalance());
    }
}
