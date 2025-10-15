SET IDENTITY_INSERT [dbo].[Genre] ON
INSERT INTO [dbo].[Genre] ([Id], [Name]) VALUES (1, N'Sci - Fi')
INSERT INTO [dbo].[Genre] ([Id], [Name]) VALUES (2, N'Drama')
INSERT INTO [dbo].[Genre] ([Id], [Name]) VALUES (3, N'Comedy')
INSERT INTO [dbo].[Genre] ([Id], [Name]) VALUES (4, N'Soap opera')	
SET IDENTITY_INSERT [dbo].[Genre] OFF

SET IDENTITY_INSERT [dbo].[Movies] ON
INSERT INTO [dbo].[Movies] ([Id], [Title], [PriceForPurchase], [QuantityForPurchase], [GenreId], [ReleaseDate], [PriceForRenting], [QuantityForRental]) VALUES (1, N'The last of us', CAST(10.00 AS Decimal(10, 2)), 5, 1, N'2023-03-15 00:00:00', CAST(1.00 AS Decimal(10, 2)),1)
INSERT INTO [dbo].[Movies] ([Id], [Title], [PriceForPurchase], [QuantityForPurchase], [GenreId], [ReleaseDate], [PriceForRenting], [QuantityForRental]) VALUES (2, N'The man in the high castle', CAST(10.00 AS Decimal(10, 2)), 100, 2, N'2015-01-15 00:00:00', CAST(1.00 AS Decimal(10, 2)), 100)
SET IDENTITY_INSERT [dbo].[Movies] OFF

SET IDENTITY_INSERT [dbo].[Rentals] ON
INSERT INTO [dbo].[Rentals] ([Id], [DeliveryAddress], [NameCustomer], [SurnameCustomer], [CostofRental], [RentalDate], [RentalDateFrom], [RentalDateTo], [PaymentMethod], [CustomerId]) VALUES (3, N'Avda. España s/n, Albacete 02071', N'Elena', N'Navarro', CAST(2.00 AS Decimal(10, 2)), N'2025-10-15 00:00:00', N'2025-12-03 00:00:00', N'2025-12-03 00:00:00', 1, N'1')
SET IDENTITY_INSERT [dbo].[Rentals] OFF


INSERT INTO [dbo].[RentalItems] ([RentalId], [MovieId], [Price], [Description]) VALUES (3, 1, CAST(1.00 AS Decimal(10, 2)), NULL)

