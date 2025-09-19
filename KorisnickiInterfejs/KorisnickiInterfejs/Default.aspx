<%@ Page Title="Početna - Evidencija sednica skupštine Srbije" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KorisnickiInterfejs.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .welcome-container {
            max-width: 600px;
            margin: 80px auto;
            padding: 50px;
            background-color: #f8f9fa;
            border: 2px solid #dee2e6;
            border-radius: 15px;
            box-shadow: 0 6px 12px rgba(0,0,0,0.15);
            text-align: center;
        }
        .welcome-header {
            margin-bottom: 40px;
            color: #2c3e50;
        }
        .welcome-header h1 {
            font-size: 32px;
            margin-bottom: 15px;
            color: #34495e;
            font-weight: bold;
        }
        .welcome-header h2 {
            font-size: 20px;
            margin-bottom: 20px;
            color: #7f8c8d;
            font-weight: normal;
        }
        .welcome-description {
            margin-bottom: 40px;
            color: #555;
            font-size: 16px;
            line-height: 1.6;
        }
        .welcome-features {
            margin-bottom: 40px;
            text-align: left;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 8px;
            border: 1px solid #e9ecef;
        }
        .welcome-features h3 {
            color: #34495e;
            margin-bottom: 15px;
            text-align: center;
        }
        .feature-list {
            list-style: none;
            padding: 0;
            margin: 0;
        }
        .feature-list li {
            padding: 8px 0;
            color: #555;
            position: relative;
            padding-left: 25px;
        }
        .feature-list li:before {
            content: "✓";
            color: #27ae60;
            font-weight: bold;
            position: absolute;
            left: 0;
        }
        .welcome-button {
            padding: 15px 40px;
            font-size: 18px;
            font-weight: bold;
            border: none;
            border-radius: 8px;
            background-color: #3498db;
            color: white;
            cursor: pointer;
            transition: all 0.3s ease;
            text-decoration: none;
            display: inline-block;
        }
        .welcome-button:hover {
            background-color: #2980b9;
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.2);
        }
        .serbian-emblem {
            font-size: 48px;
            margin-bottom: 20px;
            color: #c0392b;
        }
        .system-info {
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #ddd;
            color: #666;
            font-size: 14px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="welcome-container">
        <div class="serbian-emblem">
            🏛️ ⚖️
        </div>
        
        <div class="welcome-header">
            <h1>Dobrodošli u sistem</h1>
            <h2>Evidencija sednica skupštine Srbije</h2>
        </div>
        
        <div class="welcome-description">
            <p>Ovaj sistem omogućava efikasno upravljanje parlamentarnim aktivnostima, 
            evidenciju sednica, zasedanja i glasanja u skupštini Republike Srbije.</p>
        </div>
        
        <div class="welcome-features">
            <h3>Glavne funkcionalnosti sistema:</h3>
            <ul class="feature-list">
                <li>Upravljanje sazivima i zasedanjima</li>
                <li>Evidencija sednica i dnevnih redova</li>
                <li>Pracenje glasanja i odluka</li>
                <li>Upravljanje mandatima poslanika</li>
                <li>Generisanje izveštaja i statistika</li>
                <li>Siguran pristup sa autentifikacijom</li>
            </ul>
        </div>
        
        <div style="display: flex; gap: 20px; justify-content: center; flex-wrap: wrap;">
            <asp:Button ID="btnLogin" runat="server" Text="PRIJAVITE SE" 
                CssClass="welcome-button" OnClick="btnLogin_Click" />
        </div>
        
        <div class="system-info">
            <p><strong>Verzija sistema:</strong> 1.0.0</p>
            <p><strong>Tehnologija:</strong> ASP.NET Web Forms, C#, MS SQL Server</p>
            <p><strong>Arhitektura:</strong> 4-slojna aplikacija sa SOAP servisima</p>
            <p><strong>Autor:</strong> Uroš Vidaković IT 67/17</p>
        </div>
    </div>

</asp:Content>
