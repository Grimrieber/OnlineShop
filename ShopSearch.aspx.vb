Public Class ShopSearch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Using db As New OnlineShopContext()
                ' Include the User navigation property
                Dim shops = db.Shops.Include("Owner").ToList()

                ' Project into an anonymous object with Username + Logo
                rptShops.DataSource = shops.Select(Function(s) New With {
                    .ShopID = s.ShopID,
                    .ShopName = s.ShopName,
                    .Username = s.Owner.Username,
                    .CreatedAt = s.CreatedAt,
                    .LogoBase64 = s.LogoBase64,
                    .LogoMimeType = s.LogoMimeType
                }).ToList()

                rptShops.DataBind()
            End Using
        End If
    End Sub




End Class