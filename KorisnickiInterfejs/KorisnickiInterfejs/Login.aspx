<%@ Page Title="Prijava - Evidencija sednica skupštine Srbije" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KorisnickiInterfejs.Login1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .login-container {
            max-width: 500px;
            margin: 50px auto;
            padding: 40px;
            background-color: #f8f9fa;
            border: 2px solid #dee2e6;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .login-header {
            text-align: center;
            margin-bottom: 30px;
            color: #2c3e50;
        }
        .login-header h2 {
            font-size: 24px;
            margin-bottom: 10px;
            color: #34495e;
        }
        .login-header p {
            color: #7f8c8d;
            font-size: 14px;
        }
        .login-form-row {
            margin-bottom: 20px;
            display: flex;
            align-items: center;
        }
        .login-label {
            width: 140px;
            text-align: right;
            margin-right: 15px;
            font-weight: bold;
            color: #34495e;
        }
        .login-input {
            flex: 1;
            padding: 10px;
            border: 1px solid #bdc3c7;
            border-radius: 4px;
            font-size: 14px;
        }
        .login-input:focus {
            border-color: #3498db;
            outline: none;
            box-shadow: 0 0 5px rgba(52, 152, 219, 0.3);
        }
        .login-buttons {
            text-align: center;
            margin-top: 30px;
        }
        .login-button {
            padding: 12px 25px;
            margin: 0 10px;
            border: none;
            border-radius: 5px;
            font-size: 14px;
            font-weight: bold;
            cursor: pointer;
            transition: background-color 0.3s;
        }
        .login-button-primary {
            background-color: #27ae60;
            color: white;
        }
        .login-button-primary:hover {
            background-color: #229954;
        }
        .login-button-secondary {
            background-color: #95a5a6;
            color: white;
        }
        .login-button-secondary:hover {
            background-color: #7f8c8d;
        }
        .status-message {
            text-align: center;
            margin: 20px 0;
            padding: 10px;
            border-radius: 4px;
            font-weight: bold;
        }
        .status-error {
            background-color: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
        .serbian-emblem {
            text-align: center;
            margin-bottom: 20px;
            font-size: 32px;
            color: #c0392b;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="login-container">
        <div class="serbian-emblem">
            🏛️ ⚖️
        </div>
        <div class="login-header">
            <h2>Evidencija sednica skupštine Srbije</h2>
            <p>Prijavite se za pristup sistemu</p>
        </div>
        
        <div class="login-form-row">
            <asp:Label ID="Label1" runat="server" Text="Korisničko ime:" CssClass="login-label"></asp:Label>
            <asp:TextBox ID="KorisnickoImeTextBox" runat="server" CssClass="login-input" placeholder="Unesite korisničko ime"></asp:TextBox>
        </div>
        
        <div class="login-form-row">
            <asp:Label ID="Label2" runat="server" Text="Šifra:" CssClass="login-label"></asp:Label>
            <asp:TextBox ID="SifraTextBox" runat="server" TextMode="Password" CssClass="login-input" placeholder="Unesite šifru"></asp:TextBox>
        </div>
        
        <asp:Panel ID="StatusPanel" runat="server" Visible="false" CssClass="status-message status-error">
            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        </asp:Panel>
        
        <div class="login-buttons">
            <asp:Button ID="PrijavaButton" runat="server" onclick="PrijavaButton_Click" 
                Text="PRIJAVA" CssClass="login-button login-button-primary" />
            <asp:Button ID="OdustaniButton" runat="server" onclick="OdustaniButton_Click" 
                Text="Odustani" CssClass="login-button login-button-secondary" />
        </div>
        
        <div style="text-align: center; margin-top: 20px; padding-top: 20px; border-top: 1px solid #ddd;">
            <h4 style="color: #34495e; margin-bottom: 15px;">📋 Primeri prijave za testiranje:</h4>
            <div style="display: flex; justify-content: space-around; flex-wrap: wrap; gap: 20px; margin-bottom: 15px;">
                <div style="background: #e8f5e8; padding: 15px; border-radius: 8px; border: 1px solid #27ae60; min-width: 200px;">
                    <h5 style="color: #27ae60; margin: 0 0 10px 0;">🏛️ Predsednik</h5>
                    <p style="margin: 5px 0; font-family: monospace; color: #2c3e50;">
                        <strong>Korisničko ime:</strong><br/>aleksandar.vucic
                    </p>
                    <p style="margin: 5px 0; font-family: monospace; color: #2c3e50;">
                        <strong>Šifra:</strong><br/>aleksandar123
                    </p>
                </div>
                
                <div style="background: #fff3cd; padding: 15px; border-radius: 8px; border: 1px solid #f39c12; min-width: 200px;">
                    <h5 style="color: #f39c12; margin: 0 0 10px 0;">👑 Potpredsednik</h5>
                    <p style="margin: 5px 0; font-family: monospace; color: #2c3e50;">
                        <strong>Korisničko ime:</strong><br/>maja.gojkovic
                    </p>
                    <p style="margin: 5px 0; font-family: monospace; color: #2c3e50;">
                        <strong>Šifra:</strong><br/>maja123
                    </p>
                </div>
                
                <div style="background: #e3f2fd; padding: 15px; border-radius: 8px; border: 1px solid #2196f3; min-width: 200px;">
                    <h5 style="color: #2196f3; margin: 0 0 10px 0;">👤 Poslanik</h5>
                    <p style="margin: 5px 0; font-family: monospace; color: #2c3e50;">
                        <strong>Korisničko ime:</strong><br/>bojan.pajtic
                    </p>
                    <p style="margin: 5px 0; font-family: monospace; color: #2c3e50;">
                        <strong>Šifra:</strong><br/>bojan123
                    </p>
                </div>
            </div>
            <small style="color: #666;">Koristi ove podatke za testiranje sistema sa različitim ulogama</small>
        </div>
    </div>
</asp:Content>
