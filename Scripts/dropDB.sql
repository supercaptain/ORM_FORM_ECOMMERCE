
ALTER TABLE Product
DROP
  CONSTRAINT Product_Category_FK
GO
DROP
  TABLE Category
GO

ALTER TABLE Login
DROP
  CONSTRAINT Login_Customer_FK
GO
ALTER TABLE "Order"
DROP
  CONSTRAINT Order_Customer_FK
GO
ALTER TABLE WatchDog
DROP
  CONSTRAINT WatchDog_Customer_FK
GO
DROP
  TABLE Customer
GO

DROP TABLE Login
GO

ALTER TABLE OrderDetail
DROP
  CONSTRAINT OrderDetail_Order_FK
GO
DROP TABLE "Order"
GO

DROP
  TABLE OrderDetail
GO

ALTER TABLE OrderDetail
DROP
  CONSTRAINT OrderDetail_Product_FK
GO
ALTER TABLE ProductCostHistory
DROP
  CONSTRAINT ProductCostHistory_Product_FK
GO
ALTER TABLE WatchDog
DROP
  CONSTRAINT WatchDog_Product_FK
GO
DROP TABLE Product
GO

DROP
  TABLE ProductCostHistory
GO

ALTER TABLE "Order"
DROP
  CONSTRAINT Order_TypeDelivery_FK
GO
DROP
  TABLE TypeDelivery
GO

DROP
  TABLE WatchDog
GO