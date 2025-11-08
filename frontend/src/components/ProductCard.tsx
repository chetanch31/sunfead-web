import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { ShoppingCart, Heart } from "lucide-react";
import { Link } from "react-router-dom";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

interface ProductVariant {
  id: string;
  weight: string;
  mrp: number;
  price: number;
}

interface ProductCardProps {
  id: string;
  name: string;
  image: string;
  category: string;
  variants: ProductVariant[];
  isNew?: boolean;
  discount?: number;
}

const ProductCard = ({
  id,
  name,
  image,
  category,
  variants,
  isNew,
  discount,
}: ProductCardProps) => {
  const [selectedVariant, setSelectedVariant] = useState(variants[0]);
  const [isWishlisted, setIsWishlisted] = useState(false);

  return (
    <Card className="group overflow-hidden hover:shadow-lg transition-all duration-300 warm-glow">
      <Link to={`/product/${id}`} className="block relative">
        {/* Image */}
        <div className="aspect-square overflow-hidden bg-muted relative">
          <img
            src={image}
            alt={name}
            className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
          />
          
          {/* Badges */}
          <div className="absolute top-3 left-3 flex flex-col gap-2">
            {isNew && (
              <Badge className="bg-secondary text-secondary-foreground">
                New
              </Badge>
            )}
            {discount && (
              <Badge className="bg-accent text-accent-foreground">
                {discount}% OFF
              </Badge>
            )}
          </div>

          {/* Wishlist */}
          <Button
            size="icon"
            variant="ghost"
            className="absolute top-3 right-3 bg-background/80 hover:bg-background"
            onClick={(e) => {
              e.preventDefault();
              setIsWishlisted(!isWishlisted);
            }}
          >
            <Heart
              className={`h-4 w-4 ${
                isWishlisted ? "fill-accent text-accent" : ""
              }`}
            />
          </Button>
        </div>
      </Link>

      {/* Content */}
      <div className="p-4 space-y-3">
        <div>
          <p className="text-xs text-muted-foreground uppercase tracking-wider mb-1">
            {category}
          </p>
          <Link to={`/product/${id}`}>
            <h3 className="font-semibold text-foreground group-hover:text-primary transition-colors line-clamp-2">
              {name}
            </h3>
          </Link>
        </div>

        {/* Variant Selector */}
        <Select
          value={selectedVariant.id}
          onValueChange={(value) => {
            const variant = variants.find((v) => v.id === value);
            if (variant) setSelectedVariant(variant);
          }}
        >
          <SelectTrigger className="w-full">
            <SelectValue />
          </SelectTrigger>
          <SelectContent>
            {variants.map((variant) => (
              <SelectItem key={variant.id} value={variant.id}>
                {variant.weight}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>

        {/* Pricing */}
        <div className="flex items-baseline gap-2">
          <span className="text-xl font-bold text-foreground">
            ₹{selectedVariant.price}
          </span>
          {selectedVariant.mrp > selectedVariant.price && (
            <span className="text-sm text-muted-foreground line-through">
              ₹{selectedVariant.mrp}
            </span>
          )}
        </div>
        <p className="text-xs text-muted-foreground">Incl. of all taxes</p>

        {/* Add to Cart */}
        <Button className="w-full festival-gradient border-0 text-white" size="sm">
          <ShoppingCart className="h-4 w-4 mr-2" />
          Add to Cart
        </Button>
      </div>
    </Card>
  );
};

export default ProductCard;
