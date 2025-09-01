<%@ Page Title="Poslanik Dashboard" Language="C#" MasterPageFile="~/SednicaAdmin.Master" AutoEventWireup="true" CodeBehind="PoslanikDashboard.aspx.cs" Inherits="KorisnickiInterfejs.PoslanikDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .poslanik-welcome {
            background: linear-gradient(135deg, #27ae60, #2ecc71);
            color: white;
            padding: 30px;
            border-radius: 10px;
            margin-bottom: 30px;
            text-align: center;
        }
        .info-cards {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 20px;
            margin-bottom: 30px;
        }
        .info-card {
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            border-left: 4px solid #27ae60;
        }
        .card-icon {
            font-size: 24px;
            margin-bottom: 15px;
        }
        .card-title {
            font-size: 18px;
            font-weight: bold;
            color: #2c3e50;
            margin-bottom: 10px;
        }
        .card-description {
            color: #7f8c8d;
            line-height: 1.6;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitleContent" runat="server">
    Poslanik Dashboard
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="poslanik-welcome">
        <h2>👨‍💼 Dobrodošli, poslanici!</h2>
        <p>
            <asp:Label ID="lblWelcomeMessage" runat="server" Text=""></asp:Label>
        </p>
        <p style="margin-top: 15px; font-size: 14px; opacity: 0.9;">
            Kao poslanik, imate pristup javnim informacijama o radu skupštine
        </p>
    </div>

    <div class="info-cards">
        <div class="info-card">
            <div class="card-icon">📊</div>
            <div class="card-title">Pregled sednica</div>
            <div class="card-description">
                Pristupite kompletnom pregledu svih sednica skupštine sa mogućnostima filtriranja po sazivima i zasedanjima.
                <br><br>
                <a href="PogledajSednice.aspx" class="action-btn">Otvori pregled sednica</a>
            </div>
        </div>

        <div class="info-card">
            <div class="card-icon">📋</div>
            <div class="card-title">Pregled zasedanja</div>
            <div class="card-description">
                Pregled svih zasedanja sa mogućnostima filtriranja i detaljnim informacijama o aktivnostima.
                <br><br>
                <a href="PogledajZasedanja.aspx" class="action-btn">Otvori pregled zasedanja</a>
            </div>
        </div>

        <div class="info-card">
            <div class="card-icon">🗂️</div>
            <div class="card-title">Pregled saziva</div>
            <div class="card-description">
                Informacije o svim sazivima skupštine, njihovim periodima i aktivnostima.
                <br><br>
                <a href="PogledajSazive.aspx" class="action-btn">Otvori pregled saziva</a>
            </div>
        </div>

        <div class="info-card">
            <div class="card-icon">👤</div>
            <div class="card-title">Pregled mandata</div>
            <div class="card-description">
                Pregled svih mandata, uključujući informacije o poslanicima i njihovim pozicijama.
                <br><br>
                <a href="PogledajMandate.aspx" class="action-btn">Otvori pregled mandata</a>
            </div>
        </div>

        <div class="info-card">
            <div class="card-icon">🗳️</div>
            <div class="card-title">Istorija glasanja</div>
            <div class="card-description">
                Pristupite kompletnoj istoriji glasanja po svim pitanjima i sednicama.
                <br><br>
                <a href="IstorijaGlasanja.aspx" class="action-btn">Otvori istoriju glasanja</a>
            </div>
        </div>
    </div>

    <div class="info-card" style="border-left-color: #f39c12;">
        <div class="card-icon">ℹ️</div>
        <div class="card-title">Napomena o pristupnim dozvolama</div>
        <div class="card-description">
            Kao poslanik, imate pristup svim javnim informacijama o radu skupštine. 
            Za kreiranje novih sednica, zasedanja ili upravljanje mandatima potrebne su posebne dozvole 
            koje imaju potpredsednik i predsednik skupštine.
        </div>
    </div>
</asp:Content>
