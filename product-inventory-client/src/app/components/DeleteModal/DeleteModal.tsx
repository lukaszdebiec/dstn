import * as React from 'react';
import { useState } from 'react';
import { deleteProduct } from '../../api/api';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { Product } from '@/app/interfaces/Product';

export default function DeleteModal({
  product,
  isDeleteOpen = false,
  onClose,
  onSubmit,
}: {
  product: Product
  isDeleteOpen?: boolean;
  onClose: () => void;
  onSubmit: () => void;
}) {
  const [open, setOpen] = useState(isDeleteOpen); 

  const handleClose = () => {
    setOpen(false);
    onClose(); 
  };

  const handleDelete = async () => {
    try {
      await deleteProduct(product.id!);
      console.log('Product deleted successfully!');
      setOpen(false); 
      if (onSubmit) {
        onSubmit(); 
      }
    } catch (error) {
      console.error('Error deleting product:', error);
    }
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Confirm Product Deletion</DialogTitle>
      <DialogContent>
        <DialogContentText>
          <strong>Product: {product.name}</strong>
          <br />
          Are you sure you want to delete the selected product?
          <br />
          This action cannot be undone.
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button variant="contained" color="error" onClick={handleDelete}>
          Delete
        </Button>
      </DialogActions>
    </Dialog>
  );
}