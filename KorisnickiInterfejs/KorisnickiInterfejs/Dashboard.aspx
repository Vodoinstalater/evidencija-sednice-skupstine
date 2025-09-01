<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/SednicaAdmin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="KorisnickiInterfejs.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dashboard-welcome {
            background: linear-gradient(135deg, #3498db, #2980b9);
            color: white;
            padding: 30px;
            border-radius: 10px;
            margin-bottom: 30px;
            text-align: center;
        }
        .dashboard-stats {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 20px;
            margin-bottom: 30px;
        }
        .stat-card {
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            border-left: 4px solid #3498db;
            transition: transform 0.3s;
        }
        .stat-card:hover {
            transform: translateY(-5px);
        }
        .stat-number {
            font-size: 32px;
            font-weight: bold;
            color: #2c3e50;
            margin-bottom: 10px;
        }
        .stat-label {
            color: #7f8c8d;
            font-size: 14px;
            text-transform: uppercase;
            letter-spacing: 1px;
        }
        .quick-actions {
            background: #f8f9fa;
            padding: 25px;
            border-radius: 8px;
            border: 1px solid #dee2e6;
        }
        .quick-actions h3 {
            color: #2c3e50;
            margin-bottom: 20px;
        }
        .action-buttons {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 15px;
        }
        .action-btn {
            display: inline-block;
            padding: 15px 20px;
            background: #27ae60;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            text-align: center;
            transition: background-color 0.3s;
            font-weight: bold;
        }
        .action-btn:hover {
            background: #229954;
            color: white;
            text-decoration: none;
        }
        .recent-activity {
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            margin-top: 30px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-welcome">
        <h2>🏛️ Dobrodošli u sistem Evidencije sednica skupštine Srbije</h2>
        <p>
            <asp:Label ID="lblWelcomeMessage" runat="server" Text="Korisnik"></asp:Label>
        </p>
        <p style="margin-top: 15px; font-size: 14px; opacity: 0.9;">
            Izaberite jednu od opcija iz menija za upravljanje parlamentarnim aktivnostima
        </p>
    </div>

    <div class="dashboard-stats">
        <div class="stat-card">
            <div class="stat-number">
                <asp:Label ID="lblBrojSednica" runat="server" Text="0"></asp:Label>
            </div>
            <div class="stat-label">Ukupno sednica</div>
        </div>
        
        <div class="stat-card">
            <div class="stat-number">
                <asp:Label ID="lblBrojZasedanja" runat="server" Text="0"></asp:Label>
            </div>
            <div class="stat-label">Aktivna zasedanja</div>
        </div>
        
        <div class="stat-card">
            <div class="stat-number">
                <asp:Label ID="lblBrojMandata" runat="server" Text="0"></asp:Label>
            </div>
            <div class="stat-label">Ukupno mandata</div>
        </div>
        
        <div class="stat-card">
            <div class="stat-number">
                <asp:Label ID="lblAktivanSaziv" runat="server" Text="N/A"></asp:Label>
            </div>
            <div class="stat-label">Aktivan saziv</div>
        </div>
    </div>

    <div class="quick-actions">
        <h3>Brze akcije</h3>
        <div class="action-buttons">
            <a href="IstorijaSednica.aspx" class="action-btn">📊 Istorija sednica</a>
            <a href="IstorijaZasedanja.aspx" class="action-btn">📋 Istorija zasedanja</a>
            <a href="IstorijaSaziva.aspx" class="action-btn">🗂️ Istorija saziva</a>
            <a href="PogledajMandate.aspx" class="action-btn">👤 Pogledaj mandate</a>
            <a href="IstorijaGlasanja.aspx" class="action-btn">🗳️ Istorija glasanja</a>
        </div>
    </div>

    <div class="recent-activity">
        <h3>Poslednja aktivnost</h3>
        <asp:Label ID="lblPoslednjaAktivnost" runat="server" Text="Trenutno nema aktivnosti za prikaz." 
                   CssClass="text-muted"></asp:Label>
    </div>
</asp:Content>
