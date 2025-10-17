-- Insert an initial admin user
INSERT INTO UserInfo (FirstName, LastName, Email, Mobile, CreatedBy, CreatedOn)
VALUES ('Rahul', 'Sharma', 'rahul.sharma@example.in', '9999999999', NULL, GETDATE());

-- Insert additional users (CreatedBy and LastModifiedBy set to Rahul Sharma's UserId = 1)
INSERT INTO UserInfo (FirstName, LastName, Email, Mobile, CreatedBy, CreatedOn)
VALUES 
('Priya', 'Mehta', 'priya.mehta@example.in', '9876543210', 1, GETDATE()),
('Amit', 'Patil', 'amit.patil@example.in', '9123456789', 1, GETDATE()),
('Sneha', 'Iyer', 'sneha.iyer@example.in', '9234567890', 1, GETDATE()),
('Rohan', 'Deshmukh', 'rohan.deshmukh@example.in', '9345678901', 1, GETDATE()),
('Neha', 'Kulkarni', 'neha.kulkarni@example.in', '9456789012', 1, GETDATE());


-- Assuming UserIds are from 1 to 6 (corresponding to UserInfo insertions)
INSERT INTO UserCredentials (UserId, UserName, PrimaryPassword, SecondaryPassword, CreatedBy, CreatedOn)
VALUES 
(1, 'rahul_sharma', 'Rahul@123', 'Rahul@456', 1, GETDATE()),
(7, 'priya_mehta', 'Priya@123', 'Priya@456', 1, GETDATE()),
(8, 'amit_patil', 'Amit@123', 'Amit@456', 1, GETDATE()),
(9, 'sneha_iyer', 'Sneha@123', 'Sneha@456', 1, GETDATE()),
(10, 'rohan_deshmukh', 'Rohan@123', 'Rohan@456', 1, GETDATE()),
(11, 'neha_kulkarni', 'Neha@123', 'Neha@456', 1, GETDATE());

INSERT INTO LKPPurchaseOrderStatus (LKPPurchaseOrderStatus, ShortCode, IsActive, CreatedBy, CreatedOn)
VALUES 
('Order Placed', 'OP', 1, 1, GETDATE()),

('Advance Paid', 'AP', 1, 1, GETDATE()),

('Order Partially Received', 'PR', 1, 1, GETDATE()),

('Order Completely Received', 'CR', 1, 1, GETDATE()),

('Completely Paid', 'CP', 1, 1, GETDATE()),

('Cancelled and Closed', 'CC', 1, 1, GETDATE()),

('Completed and Closed', 'CLC', 1, 1, GETDATE());


INSERT INTO LKPProductCategories (
    ProductCategory, Description, ShortCode, IsActive, CreatedBy, CreatedOn
)
VALUES 
('Laptops', 'Various brands of portable computers', 'LP', 1, 1, GETDATE()),

('Mobile Phones', 'Smartphones and basic phones from various manufacturers', 'MB', 1, 1, GETDATE()),

('Televisions', 'LED, LCD, and Smart TVs of various sizes', 'TV', 1, 1, GETDATE()),

('Headphones', 'Wired and wireless headphones and earphones', 'HP', 1, 1, GETDATE()),

('Printers', 'Inkjet, laser, and multifunction printers', 'PR', 1, 1, GETDATE());

INSERT INTO Products (
    LKPProductCategoryId, ProductName, ProductDescription, ModelNumber,
    ShortCode, IsActive, CreatedBy, CreatedOn
)
VALUES
-- Laptops (Assume Category ID = 1)
(1, 'Dell Inspiron 15', '15-inch laptop with 12th Gen Intel processor', 'INS15-1234', 'DL1', 1, 1, GETDATE()),

-- Mobile Phones (Assume Category ID = 2)
(2, 'Samsung Galaxy S21', 'Android smartphone with 5G support', 'S21-5G', 'SGS', 1, 1, GETDATE()),

-- Televisions (Assume Category ID = 3)
(3, 'Sony Bravia 55 Inch', '4K Ultra HD Smart LED TV', 'BRV55-4K', 'SBV', 1, 1, GETDATE()),

-- Headphones (Assume Category ID = 4)
(4, 'Boat Rockerz 450', 'Wireless on-ear Bluetooth headphones', 'RKRZ450', 'BT1', 1, 1, GETDATE()),

-- Printers (Assume Category ID = 5)
(5, 'HP LaserJet Pro MFP', 'All-in-One Laser Printer', 'LJ-MFP-M227', 'HP1', 1, 1, GETDATE());

INSERT INTO PurchaseOrders (
    PurchaseOrderDate, PurchaseOrderSerialNumber, VendorId, VendorContactId,
    OrderNotes, ExpectedDeliveryDate, ActualDeliveryDate,
    LKPPurchaseOrderStatusId, PaymentTerms, InvoiceReceived,
    CreatedBy, CreatedOn
)
VALUES
(GETDATE(), 'PO-2025-001', 1, 1, 'Urgent delivery requested.', DATEADD(DAY, 7, GETDATE()), NULL, 1, '50% advance, 50% on delivery', 0, 1, GETDATE()),

(GETDATE(), 'PO-2025-002', 2, 2, 'Include installation support.', DATEADD(DAY, 10, GETDATE()), NULL, 2, 'Full payment on delivery', 0, 1, GETDATE()),

(GETDATE(), 'PO-2025-003', 3, 3, 'Part delivery allowed.', DATEADD(DAY, 15, GETDATE()), NULL, 1, 'Advance 70%, rest after', 0, 1, GETDATE()),

(GETDATE(), 'PO-2025-004', 1, 2, 'Urgent for new office.', DATEADD(DAY, 5, GETDATE()), NULL, 3, 'Net 30 days', 1, 1, GETDATE());

INSERT INTO LKPProductCategories (
    ProductCategory, Description, ShortCode, IsActive, CreatedBy, CreatedOn
)
VALUES 
('Tablets', 'Touchscreen tablet devices', 'TB', 1, 1, GETDATE()),
('Smart Watches', 'Wearable smart devices', 'SW', 1, 1, GETDATE()),
('Speakers', 'Bluetooth and wired speaker systems', 'SP', 1, 1, GETDATE()),
('Monitors', 'Computer monitors of various sizes', 'MN', 1, 1, GETDATE()),
('Keyboards', 'Wired and wireless keyboards', 'KB', 1, 1, GETDATE());

INSERT INTO Products (
    LKPProductCategoryId, ProductName, ProductDescription, ModelNumber,
    ShortCode, IsActive, CreatedBy, CreatedOn
)
VALUES
(6, 'Apple iPad Air', '10.9-inch Liquid Retina Display tablet', 'IPAIR2022', 'APA', 1, 1, GETDATE()),
(7, 'Noise ColorFit Pro 3', 'Fitness tracker and smartwatch', 'NCP3-BLK', 'NCP', 1, 1, GETDATE()),
(8, 'JBL Flip 5', 'Waterproof Portable Bluetooth Speaker', 'JBLFLIP5', 'JBL', 1, 1, GETDATE()),
(9, 'LG UltraWide 29"', '29-inch Full HD UltraWide monitor', 'LGUW29FHD', 'LGW', 1, 1, GETDATE()),
(10, 'Logitech MK345', 'Wireless keyboard and mouse combo', 'MK345-WIRE', 'LGT', 1, 1, GETDATE());

INSERT INTO PurchaseOrders (
    PurchaseOrderDate, PurchaseOrderSerialNumber, VendorId, VendorContactId,
    OrderNotes, ExpectedDeliveryDate, ActualDeliveryDate,
    LKPPurchaseOrderStatusId, PaymentTerms, InvoiceReceived,
    CreatedBy, CreatedOn
)
VALUES
(GETDATE(), 'PO-2025-005', 2, 1, 'Bulk order for tablets.', DATEADD(DAY, 6, GETDATE()), NULL, 2, 'Full advance', 0, 1, GETDATE()),
(GETDATE(), 'PO-2025-006', 3, 2, 'Include software licenses.', DATEADD(DAY, 12, GETDATE()), NULL, 1, '60% upfront', 0, 1, GETDATE()),
(GETDATE(), 'PO-2025-007', 1, 3, 'Priority for display units.', DATEADD(DAY, 8, GETDATE()), NULL, 3, 'COD', 1, 1, GETDATE()),
(GETDATE(), 'PO-2025-008', 3, 1, 'For testing purposes.', DATEADD(DAY, 14, GETDATE()), NULL, 2, 'Net 15', 0, 1, GETDATE()),
(GETDATE(), 'PO-2025-009', 2, 3, 'Immediate requirement.', DATEADD(DAY, 4, GETDATE()), NULL, 1, 'Full advance', 1, 1, GETDATE()),
(GETDATE(), 'PO-2025-010', 1, 1, 'Includes accessories.', DATEADD(DAY, 9, GETDATE()), NULL, 1, '50-50 payment terms', 0, 1, GETDATE());

-- Format: (PurchaseOrderId, ProductId, Quantity, PricePerUnit, Discount, TotalAmount, CreatedBy, CreatedOn)
INSERT INTO PurchaseOrderDetails (
    PurchaseOrderId, ProductId, Quantity, Price, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn
)
VALUES
(10, 1, 5, 55000, 1, GETDATE(), 1, GETDATE()),
(10, 2, 3, 60000, 1, GETDATE(), 1, GETDATE()),

(11, 3, 2, 45000, 1, GETDATE(), 1, GETDATE()),
(11, 4, 4, 2000, 1, GETDATE(), 1, GETDATE()),

(12, 5, 1, 15000, 1, GETDATE(), 1, GETDATE()),
(12, 6, 2, 40000, 1, GETDATE(), 1, GETDATE()),

(4, 7, 3, 3500, 1, GETDATE(), 1, GETDATE()),
(4, 8, 2, 8000, 1, GETDATE(), 1, GETDATE()),

(5, 9, 5, 12000, 1, GETDATE(), 1, GETDATE()),
(5, 10, 4, 3000, 1, GETDATE(), 1, GETDATE()),

(6, 1, 1, 55000, 1, GETDATE(), 1, GETDATE()),
(6, 6, 1, 40000, 1, GETDATE(), 1, GETDATE()),

(7, 2, 2, 60000, 1, GETDATE(), 1, GETDATE()),
(7, 4, 2, 2000, 1, GETDATE(), 1, GETDATE()),

(8, 3, 1, 45000, 1, GETDATE(), 1, GETDATE()),
(8, 7, 2, 3500, 1, GETDATE(), 1, GETDATE()),

(9, 8, 1, 8000, 1, GETDATE(), 1, GETDATE()),
(9, 10, 1, 3000, 1, GETDATE(), 1, GETDATE()),

(3, 5, 2, 15000, 1, GETDATE(), 1, GETDATE()),
(3, 9, 2, 12000, 1, GETDATE(), 1, GETDATE());

INSERT INTO PurchaseOrdersDocuments (
    PurchaseOrderId, DocumentName, DocumentFileName, Notes,
    CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn
)
VALUES
(11, 'PO Copy', 'PO_001.pdf', 'Original purchase order copy', 1, GETDATE(), 1, GETDATE()),
(11, 'Invoice', 'Invoice_001.pdf', 'Scanned invoice', 1, GETDATE(), 1, GETDATE()),

(12, 'PO Copy', 'PO_002.pdf', 'Copy of order', 1, GETDATE(), 1, GETDATE()),

(3, 'PO Copy', 'PO_003.pdf', 'PO in PDF', 1, GETDATE(), 1, GETDATE()),
(3, 'Invoice', 'Invoice_003.pdf', 'Digital invoice', 1, GETDATE(), 1, GETDATE()),

(4, 'Invoice', 'Invoice_004.pdf', 'Invoice emailed by vendor', 1, GETDATE(), 1, GETDATE()),

(5, 'PO Copy', 'PO_005.pdf', 'Finalized PO document', 1, GETDATE(), 1, GETDATE()),

(6, 'Invoice', 'Invoice_006.pdf', 'Invoice uploaded by accounts', 1, GETDATE(), 1, GETDATE()),

(7, 'PO Copy', 'PO_007.pdf', 'Printed copy', 1, GETDATE(), 1, GETDATE()),

(8, 'Invoice', 'Invoice_008.pdf', 'System-generated invoice', 1, GETDATE(), 1, GETDATE()),

(9, 'PO Copy', 'PO_009.pdf', 'Vendor-signed PO', 1, GETDATE(), 1, GETDATE()),
(9, 'Invoice', 'Invoice_009.pdf', 'With GST', 1, GETDATE(), 1, GETDATE()),

(10, 'PO Copy', 'PO_010.pdf', 'Confirmed PO PDF', 1, GETDATE(), 1, GETDATE());


-- Insert dummy vendors
INSERT INTO Vendor (VendorName, Address, City, State, PostCode, Email, Mobile, Website, CreatedBy, CreatedOn)
VALUES 
('TechSolutions Pvt Ltd', '101 Tech Street', 'Mumbai', 'Maharashtra', '400001', 'info@techsolutions.com', '9876543210', 'https://techsolutions.com', 1, GETDATE()),
('AlphaSoft Inc.', '202 Alpha Avenue', 'Bangalore', 'Karnataka', '560001', 'contact@alphasoft.in', '9123456789', 'https://alphasoft.in', 1, GETDATE()),
('BrightVision Systems', '303 Vision Lane', 'Hyderabad', 'Telangana', '500001', 'hello@brightvision.com', '9988776655', 'https://brightvision.com', 1, GETDATE());



INSERT INTO VendorContacts (Name, Designation, Email, Mobile, VendorId, CreatedBy, CreatedOn)
VALUES
('Rahul Mehta', 'Procurement Manager', 'rahul.mehta@techsolutions.com', '9876512345', 1, 1, GETDATE()),
('Anjali Desai', 'Accounts Head', 'anjali.desai@techsolutions.com', '9876523456', 1, 1, GETDATE()),
('Suresh Patil', 'Technical Lead', 'suresh.patil@techsolutions.com', '9876534567', 1, 1, GETDATE());

-- Contact for AlphaSoft Inc. (VendorId = 2)
INSERT INTO VendorContacts (Name, Designation, Email, Mobile, VendorId, CreatedBy, CreatedOn)
VALUES
('Neha Reddy', 'HR Manager', 'neha.reddy@alphasoft.in', '9123450001', 2, 1, GETDATE());

-- Contact for BrightVision Systems (VendorId = 3)
INSERT INTO VendorContacts (Name, Designation, Email, Mobile, VendorId, CreatedBy, CreatedOn)
VALUES
('Vikram Shah', 'Operations Head', 'vikram.shah@brightvision.com', '9988770001', 3, 1, GETDATE());


SELECT * FROM LKPPurchaseOrderStatus

SELECT * FROM PurchaseOrders

SELECT * FROM UserCredentials;
SELECT * FROM PurchaseOrderDetails

SELECT * FROM PurchaseOrdersDocuments

SELECT * FROM LKPPurchaseOrderStatus

DECLARE @TotalCount INT;

EXEC PurchaseOrdersGetList
    @VendorName = NULL,
    @CategoryIds = NULL,
    @ProductIds = NULL,
    @StatusIds = NULL,
    @PageNo = 1,
    @PageSize = 10,
    @SortColumn = 'PurchaseOrderDate',
    @SortDirection = 'DESC',
    @FromDate = '2025-07-01',
    @ToDate = '2025-07-02',
    @TotalCount = @TotalCount OUTPUT;

-- See total count
SELECT @TotalCount AS TotalRecords;

EXEC PurchaseOrderGetDetails 43

ALTER TABLE PurchaseOrders
ALTER COLUMN PurchaseOrderSerialNumber VARCHAR(50) NOT NULL;

SELECT * FROM PurchaseOrderErrorLogs

-- 🔸 Declare variables
DECLARE @POId INT = 0;
DECLARE @OutputPOId INT;

-- 🔸 Sample Products XML
DECLARE @ProductsXml XML = '
<Products>
    <Product>
        <ProductId>101</ProductId>
        <Quantity>5</Quantity>
        <Price>100.50</Price>
    </Product>
    <Product>
        <ProductId>102</ProductId>
        <Quantity>2</Quantity>
        <Price>500.00</Price>
    </Product>
</Products>';

-- 🔸 Sample Documents XML
DECLARE @DocumentsXml XML = '
<Documents>
    <Document>
        <DocumentName>Invoice.pdf</DocumentName>
        <DocumentFileName>PO_abc123.pdf</DocumentFileName>
        <Notes>Invoice for PO</Notes>
    </Document>
    <Document>
        <DocumentName>Specs.docx</DocumentName>
        <DocumentFileName>PO_xyz789.docx</DocumentFileName>
        <Notes>Product Specifications</Notes>
    </Document>
</Documents>';

-- 🔸 Call InsertOrUpdatePurchaseOrder
EXEC InsertOrUpdatePurchaseOrder
    @PurchaseOrderId = @POId,
    @VendorId = 1,
    @VendorContactId = 1,
    @ExpectedDeliveryDate = GETDATE(),
    @LKPPurchaseOrderStatusId = 1,
    @OrderNotes = N'This is a test order insert.',
    @PaymentTerms = 'Net 30',
    @InvoiceReceived = 0,
    @CreatedBy = 1,
    @ProductsXml = @ProductsXml,
    @DocumentsXml = @DocumentsXml,
    @CurrentPurchaseOrderId = @OutputPOId OUTPUT;

-- 🔸 Show result
PRINT 'Inserted PurchaseOrderId: ' + CAST(@OutputPOId AS VARCHAR);
