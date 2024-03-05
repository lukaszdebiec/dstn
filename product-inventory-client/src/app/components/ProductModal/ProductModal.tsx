import * as React from 'react';
import { useState, useEffect } from 'react';
import { Product } from '@/app/interfaces/Product';
import { createProduct, updateProduct } from '@/app/api/api';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import TextField from '@mui/material/TextField';

export default function ProductModal({
  product,
  isEditOpen = false,
  isCreate = false,
  onClose,
  onSubmit,
}: {
  product: Product;
  isEditOpen?: boolean;
  isCreate?: boolean;
  onClose: () => void;
  onSubmit: () => void;
}) {
  const [formData, setFormData] = useState<Product>(product || { name: '', brand: '', price: 0 });
  const [open, setOpen] = useState(isEditOpen || isCreate);

  useEffect(() => {
    setOpen(isEditOpen || isCreate);
    if (!isCreate) { 
      setFormData(product);
    }
  }, [isEditOpen, isCreate, product]);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [event.target.name]: event.target.value,
    });
  };

  const handleClose = () => {
    setOpen(false);
    onClose();
  };

  const handleSubmit = async () => {
    try {
      if (!formData.name || !formData.brand || formData.price === 0) {
        console.error('Please fill out all required fields and ensure the price is greater than 0.');
        return;
      }
      if (isCreate) {
        await createProduct(formData); 
        console.log('Product created successfully!');
      } else {
        await updateProduct(product.id!, formData); 
        console.log('Product updated successfully!');
      }
      setOpen(false);
      onSubmit();
    } catch (error) {
      console.error(`Error ${isCreate ? 'creating' : 'updating'} product:`, error);
    }
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>{isCreate ? 'Create Product' : 'Edit Product'}</DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          margin="dense"
          label="Name"
          type="text"
          fullWidth
          value={formData.name}
          onChange={handleInputChange}
          name="name"
        />
        <TextField
          margin="dense"
          label="Brand"
          type="text"
          fullWidth
          value={formData.brand}
          onChange={handleInputChange}
          name="brand"
        />
        <TextField
          margin="dense"
          label="Price"
          type="number"
          fullWidth
          value={formData.price}
          onChange={handleInputChange}
          name="price"
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button type="submit" onClick={handleSubmit} disabled={!formData.name || !formData.brand || formData.price < 1}>
          {isCreate ? 'Create' : 'Save'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
