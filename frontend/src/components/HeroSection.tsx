import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";
import heroImage from "@/assets/hero-banner.jpg";

const HeroSection = () => {
  return (
    <section className="relative min-h-[70vh] md:min-h-[80vh] flex items-center overflow-hidden">
      {/* Background Image */}
      <div 
        className="absolute inset-0 bg-cover bg-center"
        style={{ backgroundImage: `url(${heroImage})` }}
      >
        <div className="absolute inset-0 bg-gradient-to-r from-background/95 via-background/80 to-background/40" />
      </div>

      {/* Content */}
      <div className="container mx-auto px-4 relative z-10">
        <div className="max-w-2xl">
          <h1 className="text-4xl md:text-6xl lg:text-7xl font-display font-bold mb-6 leading-tight">
            Taste the
            <span className="block text-gradient">Tradition</span>
            in Every Bite
          </h1>
          
          <p className="text-lg md:text-xl text-muted-foreground mb-8 max-w-xl">
            From our kitchens to your celebrations. Authentic Indian snacks and sweets 
            crafted with love, heritage, and the finest ingredients.
          </p>

          <div className="flex flex-col sm:flex-row gap-4">
            <Link to="/shop">
              <Button size="lg" variant="outline" className="border-2">
                Explore All Products
              </Button>
            </Link>
            <Link to="/bulk-order">
              <Button size="lg" className="festival-gradient border-0 text-white">
                Bulk Orders
              </Button>
            </Link>
          </div>

          {/* Trust Badges */}
          <div className="flex flex-wrap gap-6 mt-12 pt-8 border-t border-border/50">
            <div className="flex items-center gap-2">
              <div className="h-8 w-8 rounded-full bg-primary/10 flex items-center justify-center">
                ✓
              </div>
              <span className="text-sm font-medium">100% Fresh</span>
            </div>
            <div className="flex items-center gap-2">
              <div className="h-8 w-8 rounded-full bg-primary/10 flex items-center justify-center">
                ✓
              </div>
              <span className="text-sm font-medium">No Preservatives</span>
            </div>
            <div className="flex items-center gap-2">
              <div className="h-8 w-8 rounded-full bg-primary/10 flex items-center justify-center">
                ✓
              </div>
              <span className="text-sm font-medium">Traditional Recipes</span>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default HeroSection;
