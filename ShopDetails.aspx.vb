Imports System.Data.Entity

Public Class ShopDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim shopIdString As String = Request.QueryString("ShopID")
            Dim shopID As Guid

            If Not String.IsNullOrEmpty(shopIdString) AndAlso Guid.TryParse(shopIdString, shopID) Then
                LoadShop(shopID)
            Else
                Response.Write("Invalid or missing shop ID.")
            End If
        End If
    End Sub

    Private Sub LoadShop(shopID As Guid)
        Using db As New OnlineShopContext()
            Dim shop = db.Shops.Find(shopID)
            If shop IsNot Nothing Then
                ' ===== Shop Info =====
                lblShopName.Text = shop.ShopName
                lblOwner.Text = db.Users.Find(shop.UserID)?.Username
                lblCreatedAt.Text = shop.CreatedAt.ToString("yyyy-MM-dd")
                lblDescription.Text = shop.Description

                ' Theme
                If Not String.IsNullOrEmpty(shop.ThemeColor) Then
                    btnFollow.CssClass = "btn btn-sm"
                    btnFollow.Style("background-color") = shop.ThemeColor
                    btnFollow.Style("border-color") = shop.ThemeColor
                    btnFollow.Style("color") = "#fff"
                End If


                ' Logo
                If Not String.IsNullOrEmpty(shop.LogoBase64) Then
                    imgShopLogo.ImageUrl = $"data:{shop.LogoMimeType};base64,{shop.LogoBase64}"
                End If

                ' Banner
                If Not String.IsNullOrEmpty(shop.BannerBase64) Then
                    divShopBanner.Style("background-image") = $"url('data:{shop.BannerMimeType};base64,{shop.BannerBase64}')"
                    divShopBanner.Style("background-size") = "cover"
                    divShopBanner.Style("background-position") = "center"
                End If


                ' ===== Shop Products =====
                Dim products = db.Products.Where(Function(p) p.ShopID = shopID) _
                                          .Include("ProductImages") _
                                          .ToList()

                Dim productList = products.Select(Function(p)
                                                      Dim mainImage = p.ProductImages.FirstOrDefault()
                                                      Dim imageBase64 As String = Nothing

                                                      If mainImage IsNot Nothing AndAlso Not String.IsNullOrEmpty(mainImage.ImageData) Then
                                                          ' Directly embed the base64 data like on ProductDetails.aspx
                                                          imageBase64 = $"data:image/png;base64,{mainImage.ImageData}"
                                                      End If

                                                      Return New With {
                                                          .ProductID = p.ProductID,
                                                          .ProductName = p.ProductName,
                                                          .Price = p.Price,
                                                          .ImageBase64 = imageBase64
                                                      }
                                                  End Function).ToList()

                rptShopProducts.DataSource = productList
                rptShopProducts.DataBind()
            Else
                Response.Write("Shop not found.")
            End If
        End Using
    End Sub

End Class
