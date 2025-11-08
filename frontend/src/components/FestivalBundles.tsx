import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Gift, Sparkles } from "lucide-react";
import festivalImg from "@/assets/festival-bundle.jpg";

const bundles = [
  {
    id: "diwali-special",
    name: "Diwali Celebration Box",
    description: "12 varieties of premium sweets and namkeens",
    price: 1499,
    mrp: 1999,
    items: 12,
    image: festivalImg,
  },
  {
    id: "holi-hamper",
    name: "Holi Festival Hamper",
    description: "Colorful sweets and crunchy snacks collection",
    price: 1299,
    mrp: 1699,
    items: 10,
    image: festivalImg,
  },
  {
    id: "rakhi-gift",
    name: "Raksha Bandhan Gift Pack",
    description: "Brother's favorite savory and sweet treats",
    price: 999,
    mrp: 1299,
    items: 8,
    image: festivalImg,
  },
];

const FestivalBundles = () => {
  return (
    <section className="py-16 md:py-24 pattern-bg">
      <div className="container mx-auto px-4">
        <div className="text-center mb-12">
          <div className="inline-flex items-center gap-2 mb-4">
            <Sparkles className="h-6 w-6 text-primary" />
            <Badge className="bg-primary text-primary-foreground">
              Limited Edition
            </Badge>
            <Sparkles className="h-6 w-6 text-primary" />
          </div>
          <h2 className="text-3xl md:text-4xl lg:text-5xl font-display font-bold mb-4">
            Festival Gift Bundles
          </h2>
          <p className="text-lg text-muted-foreground max-w-2xl mx-auto">
            Curated gift hampers perfect for every celebration
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {bundles.map((bundle) => (
            <Card
              key={bundle.id}
              className="overflow-hidden group hover:shadow-xl transition-all duration-300"
            >
              <div className="relative aspect-square overflow-hidden">
                <img
                  src={bundle.image}
                  alt={bundle.name}
                  className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                />
                <div className="absolute top-4 right-4">
                  <Badge className="bg-accent text-accent-foreground">
                    Save {Math.round(((bundle.mrp - bundle.price) / bundle.mrp) * 100)}%
                  </Badge>
                </div>
              </div>
              
              <div className="p-6 space-y-4">
                <div>
                  <h3 className="text-xl font-display font-bold mb-2">
                    {bundle.name}
                  </h3>
                  <p className="text-muted-foreground text-sm">
                    {bundle.description}
                  </p>
                </div>

                <div className="flex items-center gap-2 text-sm text-muted-foreground">
                  <Gift className="h-4 w-4" />
                  <span>{bundle.items} items included</span>
                </div>

                <div className="flex items-baseline gap-2 pt-2 border-t">
                  <span className="text-2xl font-bold text-foreground">
                    ₹{bundle.price}
                  </span>
                  <span className="text-sm text-muted-foreground line-through">
                    ₹{bundle.mrp}
                  </span>
                </div>

                <Button className="w-full festival-gradient border-0 text-white">
                  View Bundle
                </Button>
              </div>
            </Card>
          ))}
        </div>
      </div>
    </section>
  );
};

export default FestivalBundles;
