SET IDENTITY_INSERT [dbo].[Genre] ON
INSERT INTO [dbo].[Genre] ([Id], [Name]) VALUES (1, N'Comedy')
INSERT INTO [dbo].[Genre] ([Id], [Name]) VALUES (2, N'Drama')
SET IDENTITY_INSERT [dbo].[Genre] OFF

SET IDENTITY_INSERT [dbo].[Movies] ON
INSERT INTO [dbo].[Movies] ([Id], [Title], [PriceForPurchase], [QuantityForPurchase], [GenreId], [ReleaseDate], [PriceForRenting], [QuantityForRental]) VALUES (1, N'The man in the high castle', CAST(10.00 AS Decimal(10, 2)), 100, 2, N'2020-01-01 00:00:00', CAST(1.00 AS Decimal(10, 2)), 100)
SET IDENTITY_INSERT [dbo].[Movies] OFF
