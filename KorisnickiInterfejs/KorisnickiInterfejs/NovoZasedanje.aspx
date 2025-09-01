<%@ Page Title="" Language="C#" MasterPageFile="~/SednicaAdmin.Master" AutoEventWireup="true" CodeBehind="NovoZasedanje.aspx.cs" Inherits="KorisnickiInterfejs.NovoZasedanje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .content-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px;
            border-radius: 15px;
            margin-bottom: 30px;
            text-align: center;
        }

        .content-header h2 {
            margin: 0 0 10px 0;
            font-size: 2.5em;
            font-weight: 300;
        }

        .content-header p {
            margin: 0;
            font-size: 1.2em;
            opacity: 0.9;
        }

        .form-container {
            background: white;
            border-radius: 15px;
            padding: 40px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.1);
            border: 1px solid #e9ecef;
            max-width: 600px;
            margin: 0 auto;
        }

        .form-group {
            margin-bottom: 25px;
        }

        .form-group label {
            display: block;
            margin-bottom: 10px;
            color: #495057;
            font-weight: 600;
            font-size: 16px;
        }

        .form-control {
            width: 100%;
            padding: 15px 20px;
            border: 2px solid #e9ecef;
            border-radius: 10px;
            font-size: 16px;
            transition: all 0.3s ease;
            box-sizing: border-box;
        }

        .form-control:focus {
            outline: none;
            border-color: #667eea;
            box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.1);
            transform: translateY(-2px);
        }

        .btn {
            padding: 15px 30px;
            border: none;
            border-radius: 10px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            text-decoration: none;
            display: inline-block;
            text-align: center;
            min-width: 150px;
        }

        .btn-primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }

        .btn-primary:hover {
            transform: translateY(-3px);
            box-shadow: 0 10px 30px rgba(102, 126, 234, 0.3);
        }

        .btn-secondary {
            background: #6c757d;
            color: white;
            margin-left: 15px;
        }

        .btn-secondary:hover {
            background: #5a6268;
            transform: translateY(-3px);
        }

        .current-session-info {
            background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
            color: white;
            padding: 25px;
            border-radius: 15px;
            margin-bottom: 30px;
            text-align: center;
        }

        .current-session-info h3 {
            margin: 0 0 20px 0;
            font-size: 1.4em;
        }

        .session-details {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
        }

        .detail-item {
            background: rgba(255,255,255,0.1);
            padding: 20px;
            border-radius: 10px;
            backdrop-filter: blur(10px);
        }

        .detail-label {
            font-size: 0.9em;
            opacity: 0.8;
            margin-bottom: 8px;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .detail-value {
            font-size: 1.2em;
            font-weight: 600;
        }

        .alert {
            padding: 20px;
            border-radius: 10px;
            margin-bottom: 25px;
            border-left: 5px solid;
            font-weight: 500;
        }

        .alert-success {
            background: #d4edda;
            border-color: #28a745;
            color: #155724;
        }

        .alert-danger {
            background: #f8d7da;
            border-color: #dc3545;
            color: #721c24;
        }

        .alert-info {
            background: #d1ecf1;
            border-color: #17a2b8;
            color: #0c5460;
        }

        .form-actions {
            text-align: center;
            margin-top: 30px;
            padding-top: 30px;
            border-top: 2px solid #e9ecef;
        }

        .help-text {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 10px;
            margin-top: 30px;
            border-left: 4px solid #17a2b8;
        }

        .help-text h4 {
            color: #17a2b8;
            margin: 0 0 15px 0;
        }

        .help-text p {
            color: #6c757d;
            margin: 0;
            line-height: 1.6;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <h2>🎯 Otvori Novo Zasedanje</h2>
                    <p>Kreiraj novo zasedanje za aktivan saziv skupštine</p>
    </div>

    <!-- Current Session Info -->
    <div class="current-session-info">
        <h3>📋 Trenutno Aktivno Zasedanje</h3>
        <div class="session-details">
            <div class="detail-item">
                <div class="detail-label">Naziv Zasedanja</div>
                <div class="detail-value">
                    <asp:Label ID="lblTrenutnoZasedanje" runat="server" Text="Nema aktivnog zasedanja" />
                </div>
            </div>
            <div class="detail-item">
                <div class="detail-label">Tip Zasedanja</div>
                <div class="detail-value">
                    <asp:Label ID="lblTipZasedanja" runat="server" Text="-" />
                </div>
            </div>
            <div class="detail-item">
                <div class="detail-label">Saziv</div>
                <div class="detail-value">
                    <asp:Label ID="lblSaziv" runat="server" Text="-" />
                </div>
            </div>
            <div class="detail-item">
                <div class="detail-label">Status</div>
                <div class="detail-value">
                    <asp:Label ID="lblStatus" runat="server" Text="-" />
                </div>
            </div>
        </div>
    </div>

    <!-- Form Container -->
    <div class="form-container">
        <h3 style="text-align: center; margin-bottom: 30px; color: #495057;">🆕 Kreiraj Novo Zasedanje</h3>
        
        <div class="form-group">
            <label for="txtNazivZasedanja">Naziv Zasedanja:</label>
            <asp:TextBox ID="txtNazivZasedanja" runat="server" CssClass="form-control" 
                       placeholder="Unesite naziv zasedanja..." />
            <asp:RequiredFieldValidator ID="rfvNazivZasedanja" runat="server" 
                                      ControlToValidate="txtNazivZasedanja"
                                      ErrorMessage="Naziv zasedanja je obavezan!" 
                                      Display="Dynamic" ForeColor="#dc3545" />
        </div>
        
        <div class="form-group">
            <label for="ddlTipZasedanja">Tip Zasedanja:</label>
            <asp:DropDownList ID="ddlTipZasedanja" runat="server" CssClass="form-control">
                <asp:ListItem Text="-- Izaberite tip --" Value="" />
                <asp:ListItem Text="Redovno" Value="1" />
                <asp:ListItem Text="Vanredno" Value="2" />
                <asp:ListItem Text="Hitan" Value="3" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvTipZasedanja" runat="server" 
                                      ControlToValidate="ddlTipZasedanja"
                                      ErrorMessage="Tip zasedanja je obavezan!" 
                                      Display="Dynamic" ForeColor="#dc3545" />
        </div>

        <div class="form-actions">
            <asp:Button ID="btnKreirajZasedanje" runat="server" Text="🆕 Kreiraj Zasedanje" 
                       CssClass="btn btn-primary" OnClick="btnKreirajZasedanje_Click" />
            <asp:Button ID="btnResetuj" runat="server" Text="🔄 Resetuj" 
                       CssClass="btn btn-secondary" OnClick="btnResetuj_Click" />
        </div>
    </div>

    <!-- Help Text -->
    <div class="help-text">
        <h4>💡 Kako funkcioniše?</h4>
        <p>
            Kada kreirate novo zasedanje, ono se automatski povezuje sa trenutno aktivnim sazivom skupštine. 
            Novo zasedanje omogućava organizovanje sednica i glasanja. Nakon kreiranja zasedanja, možete 
            kreirati sednice sa pitanjima za glasanje.
        </p>
    </div>

    <!-- Alerts -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lblAlertMessage" runat="server" />
    </asp:Panel>
</asp:Content>
